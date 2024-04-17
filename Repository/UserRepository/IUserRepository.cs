using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task CreateUser(UserModel model);
        Task UpdateUser(string Id,UserUpdateModel model);
        Task DeleteUser(string Id);
        Task<List<UserDTO>> GetAllUser(string? search,string? groupPermissionId,int? page,int? pageSize);
        Task<UserDTO?> GetUser(string Id);
        Task ResetPassword(string email);
    }
}
