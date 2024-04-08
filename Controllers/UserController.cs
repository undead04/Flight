using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository) 
        { 
            this.userRepository=userRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUser(string? search)
        {
            try
            {
                var users = await userRepository.GetAllUser(search);
                return Ok(Repository<List<UserDTO>>.WithData(users, 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{Id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(string Id)
        {
            try
            {
                var user = await userRepository.GetUser(Id);
                if(user==null)
                {
                    return NotFound();
                }
                return Ok(Repository<UserDTO>.WithData(user, 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserModel model)
        {
            try
            {
                await userRepository.CreateUser(model);
                return Ok(Repository<string>.WithMessage("Tạo thành công người dùng", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{Id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            try
            {
               
                var user = await userRepository.GetUser(Id);
                if (user == null)
                {
                    return NotFound();
                }
                await userRepository.DeleteUser(Id);
                return Ok(Repository<string>.WithMessage("Xóa thành công người dùng", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{Id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string Id,UserUpdateModel model)
        {
            try
            {

                var user = await userRepository.GetUser(Id);
                if (user == null)
                {
                    return NotFound();
                }
                await userRepository.UpdateUser(Id,model);
                return Ok(Repository<string>.WithMessage("Cập nhập thành công người dùng", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
