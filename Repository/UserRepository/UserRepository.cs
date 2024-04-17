using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Flight.Service.FileService;
using Flight.Service.PaginationService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Flight.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<GroupPermission> roleManager;
        private readonly IPaginationService paginationServer;

        public UserRepository(UserManager<ApplicationUser> userManager,RoleManager<GroupPermission> roleManager,IPaginationService paginationServer) 
        { 
            this.userManager= userManager;
            this.roleManager = roleManager;
            this.paginationServer= paginationServer;
        }
        public async Task CreateUser(UserModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Name,
                Email = model.Email,
                PhoneNumber = model.Phone,
            };
            var result = await userManager.CreateAsync(user, model.Password);
        }

        public async Task DeleteUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
        }

        public async Task<List<UserDTO>> GetAllUser(string? search, string? groupPermissionId, int? page, int? pageSize)
        {
            var users = userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(us => us.Email.Contains(search));
            }
            if (!string.IsNullOrEmpty(groupPermissionId))
            {
                
                var roleName = await roleManager.FindByIdAsync(groupPermissionId);
                var usersInRole = new List<ApplicationUser>();

                foreach (var user in users)
                {
                    var isInRole = await userManager.IsInRoleAsync(user, roleName.Name);
                    if (isInRole)
                    {
                        usersInRole.Add(user);
                    }
                }
            }
            if(page.HasValue&&pageSize.HasValue)
            {
                users = await paginationServer.Pagination<ApplicationUser>(users, page.Value, pageSize.Value);
            }
                return users.Select(us=>new UserDTO
            {
                Id=us.Id,
                Email=us.Email,
                Name=us.UserName,
                Phone=us.PhoneNumber,
                Permission= string.Join(string.Empty, userManager.GetRolesAsync(us).Result)
            }).ToList();
        }

        public async Task<UserDTO?> GetUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return null;
            }
            return new UserDTO
            {
                Id=user.Id,
                Email = user.Email,
                Name = user.UserName,
                Phone=user.PhoneNumber,
            };
        }

        public Task ResetPassword(string email)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUser(string Id, UserUpdateModel model)
        {
            var user = await userManager.FindByIdAsync(Id);
            if(user!=null)
            {
                user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                user.UserName = model.Name;
                await userManager.UpdateAsync(user);
            }
        }

        
    }
}
