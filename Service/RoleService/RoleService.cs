using Flight.Data;
using Microsoft.AspNetCore.Identity;

namespace Flight.Service.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<GroupPermission> roleManager;

        public RoleService(UserManager<ApplicationUser> userManager,RoleManager<GroupPermission> roleManager) 
        { 
            this.userManager=userManager;
            this.roleManager = roleManager;
        }    
        public async Task AddMemberRole(string userId, string roleId)
        {
            var user=await userManager.FindByIdAsync(userId);
            var role=await roleManager.FindByIdAsync(roleId);
            if(await roleManager.RoleExistsAsync(role.Name))
            {
                await userManager.AddToRoleAsync(user, role.Name);
            }
            
        }

        public async Task DeleteRoleMember(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var roleName = await userManager.GetRolesAsync(user);
            if(user!= null) 
            {
                await userManager.RemoveFromRolesAsync(user,roleName);
            }
        }
    }
}
