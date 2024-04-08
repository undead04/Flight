using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace Flight.Repository.PermissionRepository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly MyDbContext context;

        public PermissionRepository(MyDbContext context) 
        { 
            this.context=context;   
        }

        public async Task CreatePermission(PermissionModel model)
        {
            var permission = new PermissionDocumentType
            {
                DocumnetTypeId = model.DocumentTypeId,
                GroupPermissionId = model.GroupPermissionId,
                ClaimsType = model.ClaimsType,
                ClaimsValue = model.ClaimsValue
            };
            await context.permissionDocuments.AddAsync(permission);
            await context.SaveChangesAsync();

        }

        public async Task DeletePermission(int documnetTypeId)
        {
            var permission = await context.permissionDocuments.Where(per => per.DocumnetTypeId == documnetTypeId).ToListAsync();
            if(permission != null)
            {
                context.permissionDocuments.RemoveRange(permission);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<PermissionDTO>?> GetPermission(int documnetTypeId)
        {
            var permission = await context.permissionDocuments.Where(per => per.DocumnetTypeId == documnetTypeId).ToListAsync();
            if (permission==null)
            {
                return null;
            }
            return permission.Select(per =>new PermissionDTO
            {
                Id=per.Id,
                DocumnetTypeId=per.DocumnetTypeId,
                GroupPermissionId=per.GroupPermissionId,
                ClaimsType=per.ClaimsType,
                ClaimsValue=per.ClaimsValue,
            }).ToList();
        }
    }
}
