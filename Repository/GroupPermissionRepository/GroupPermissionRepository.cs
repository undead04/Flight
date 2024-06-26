﻿using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Flight.Service.PaginationService;
using Flight.Service.ReadTokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Flight.Repository.GroupPermissionRepository
{
    public class GroupPermissionRepository : IGroupPermissionRepository
    {
        private readonly RoleManager<GroupPermission> roleManager;
        private readonly MyDbContext context;
        private readonly IReadTokenService readTokenService;
        private readonly IPaginationService paginationServer;

        public GroupPermissionRepository(RoleManager<GroupPermission> roleManager,MyDbContext context,IReadTokenService readTokenService,
            IPaginationService paginationServer) 
        {
            this.roleManager = roleManager;
            this.context = context;
            this.readTokenService = readTokenService;
            this.paginationServer = paginationServer;
        }
        public async Task<IdentityResult> CreateGroupPermission(GroupPermissionModel model)
        {
            var userId = await readTokenService.ReadJWT();
            var groupPermission = new GroupPermission
            {
                Name = model.Name,
                Note = model.Note,
                Create_Date = DateTime.Now.Date,
                CreateUserId = userId
            };
            var role=await roleManager.CreateAsync(groupPermission);
            return role;
        }

        public async Task DeleteGroupPermission(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);

            
        }

        public async Task<List<GroupPermissionDTO>> GetAllGroupPermission(string? search, int? page, int? pageSize)
        {
            var role = roleManager.Roles.Include(f=>f.ApplicationUser).AsQueryable();
            if(!string.IsNullOrEmpty(search))
            {
                role=role.Where(role => role.Name.Contains(search));
            }
            if(page.HasValue&&pageSize.HasValue)
            {
                role = await paginationServer.Pagination<GroupPermission>(role, page.Value, pageSize.Value);
            }
            return await role.Select(ro => new GroupPermissionDTO
            {
                Name=ro.Name,
                Create_At=ro.Create_Date,
                Creator=ro.ApplicationUser!=null?ro.ApplicationUser.UserName:string.Empty,
                Id=ro.Id,
                Note=ro.Note,
                TotalMember = context.UserRoles.Count(us => us.RoleId == ro.Id)

            }).ToListAsync();
        }

        public async Task<GroupPermissionDTO?> GetGroupPermission(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if(role!=null)
            {
                return new GroupPermissionDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Create_At = role.Create_Date,
                    Creator = role.ApplicationUser != null ? role.ApplicationUser.UserName : string.Empty,
                    TotalMember = context.UserRoles.Where(us => us.RoleId == role.Id).ToList().Count(),
                    Note = role.Note,
                };
            }
            return null;
        }

        public async Task<IdentityResult> UpdateGroupPermission(string id, GroupPermissionModel model)
        {
            var role = await roleManager.FindByIdAsync(id);
            role.Name = model.Name;
            role.Note = model.Note;
            var result= await roleManager.UpdateAsync(role);
            return result;
        }
    }
}
