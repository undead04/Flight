using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Flight.Service.FileService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Flight.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileService fileService;

        public UserRepository(UserManager<ApplicationUser> userManager,IFileService fileService) 
        { 
            this.userManager= userManager;
            this.fileService = fileService;
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

        public async Task<List<UserDTO>> GetAllUser(string? search)
        {
            var users = await userManager.Users.ToListAsync();
            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(us => us.Email.Contains(search)).ToList();
            }
            return users.Select(us=>new UserDTO
            {
                Id=us.Id,
                Email=us.Email,
                Name=us.UserName,
                Phone=us.PhoneNumber,
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
