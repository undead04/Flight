using Flight.Model;
using Flight.Model.DTO;
using Microsoft.AspNetCore.Identity;

namespace Flight.Repository.GroupPermissionRepository
{
    public interface IGroupPermissionRepository
    {
        Task<IdentityResult> CreateGroupPermission(GroupPermissionModel model);
        Task<IdentityResult> UpdateGroupPermission(string id,GroupPermissionModel model);
        Task DeleteGroupPermission(string id);
        Task<List<GroupPermissionDTO>> GetAllGroupPermission(string?search);
        Task<GroupPermissionDTO?> GetGroupPermission(string id);
    }
}
