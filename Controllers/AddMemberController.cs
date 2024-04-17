using Flight.Model;
using Flight.Service.RoleService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Flight.Model.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddMemberController : ControllerBase
    {
        private readonly IRoleService roleService;

        public AddMemberController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        [HttpPost("{userId}/{roleId}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> AddMemberGroup(string userId,string roleId)
        {
            try
            {
                await roleService.AddMemberRole(userId, roleId);
                return Ok(Repository<string>.WithMessage("Thành công thêm vào nhóm", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{userId}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteMemberGroup(string userId)
        {
            try
            {
                await roleService.DeleteRoleMember(userId);
                return Ok(Repository<string>.WithMessage("Xóa khỏi nhóm thành công", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
