using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Repository.PermissionRepository
{
    public interface IPermissionRepository
    {
        Task CreatePermission(PermissionModel model);
        Task DeletePermission(int DocumnetTypeId);
        Task<List<PermissionDTO>?> GetPermission(int DocumnetTypeId);
    }
}
