using Flight.Data;
using Flight.Model;
using Flight.Service.RoleService;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;

namespace Flight.Service.ChangeOwnerService
{
    public class ChangeOwerService : IChangeOwnerService
    {
        private UserManager<ApplicationUser> userManager;
        private readonly IRoleService roleService;
        private readonly RoleManager<GroupPermission> roleManager;

        public ChangeOwerService(UserManager<ApplicationUser> userManager,IRoleService roleService,RoleManager<GroupPermission> roleManager) 
        {
            this.userManager = userManager;
            this.roleService = roleService;
            this.roleManager = roleManager;


        }  
        public async Task<bool> ChangeOwerSystem(string userId)
        {
            var ownerSystem=await userManager.GetUsersInRoleAsync(AppRoleModel.Admin);
            var user = await userManager.FindByIdAsync(userId);
            var roleAdmin=await roleManager.FindByNameAsync(AppRoleModel.Admin);
            if(ownerSystem!=null)
            {
                await roleService.DeleteRoleMember(ownerSystem.First().Id);
                await roleService.DeleteRoleMember(user.Id);
                await roleService.AddMemberRole(userId, roleAdmin.Id);
                return true;
            }
            return false;
        }

        public async Task<bool> ComfimChangOwerSystem(string userId, string password)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool isComfirm = await userManager.CheckPasswordAsync(user, password);
            return isComfirm;
        }
    }
}
