using Flight.Data;
using Flight.Service.ReadTokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Flight.Authorize.DocumentAuthorize
{
    public class DocumentAuthorize : IDocumentAuthorize
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext context;
        private readonly IReadTokenService readTokenService;
        private readonly RoleManager<GroupPermission> roleManager;

        public DocumentAuthorize(UserManager<ApplicationUser> userManager,MyDbContext context,IReadTokenService readTokenService,
            RoleManager<GroupPermission> roleManager) 
        {
            this.userManager = userManager;
            this.context = context;
            this.readTokenService = readTokenService;
            this.roleManager = roleManager;
        }

        public async Task<bool> EditDocumentAuthorize(int id)
        {
            var userId = await readTokenService.ReadJWT();
            var user = await userManager.FindByIdAsync(userId);
            var documentFlight = await context.documentFlight
               .Include(f => f.DocumentFlightPermissions)!
               .ThenInclude(f => f.GroupPermission)
               .Include(f => f.DocumentType)
               .ThenInclude(f => f!.PermissionDocuments)
               .FirstOrDefaultAsync(doc => doc.Id == id);
            var groupPermissionDocument = documentFlight!.DocumentFlightPermissions;
            var permissionDocument = documentFlight!.DocumentType!.PermissionDocuments;
            var role = await userManager.GetRolesAsync(user);
            var roleName = string.Join(string.Empty, role);
            if (roleName == "admin") return true;
            foreach (var GroupPermission in groupPermissionDocument!)
            {
                if (GroupPermission.GroupPermission!.Name == roleName)
                {
                    foreach (var permission in permissionDocument!)
                    {
                        if (permission.ClaimsType == roleName)
                        {
                            return permission.ClaimsValue.Contains("edit");
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> ReadDocumentAuthorize(int id)
        {
            var userId = await readTokenService.ReadJWT();
            var user=await userManager.FindByIdAsync(userId);
            var documentFlight = await context.documentFlight
                .Include(f=>f.DocumentFlightPermissions)!
                .ThenInclude(f=>f.GroupPermission)
                .Include(f=>f.DocumentType)
                .ThenInclude(f=>f!.PermissionDocuments)
                .FirstOrDefaultAsync(doc => doc.Id == id);
            var groupPermissionDocument = documentFlight!.DocumentFlightPermissions;
            var permissionDocument=documentFlight!.DocumentType!.PermissionDocuments;
            var role = await userManager.GetRolesAsync(user);
            var roleName = string.Join(string.Empty, role);
            if (roleName == "admin") return true;
            foreach (var GroupPermission in groupPermissionDocument!)
            {
                if(GroupPermission.GroupPermission!.Name==roleName)
                {
                    foreach(var permission in permissionDocument!)
                    {
                        if (permission.ClaimsType == roleName)
                        {
                            return permission.ClaimsValue.Contains("read");
                        }
                    }
                }
            }
            return false;
        }
    }
}
