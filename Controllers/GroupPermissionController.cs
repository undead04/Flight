using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.GroupPermissionRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupPermissionController : ControllerBase
    {
        private readonly IGroupPermissionRepository groupPerMissionRepository;

        public GroupPermissionController(IGroupPermissionRepository groupPermissionRepository) 
        { 
            this.groupPerMissionRepository=groupPermissionRepository;
        }
        [HttpGet]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetAllGroupPermission(string? search, int? page, int? pageSize) 
        {
            try
            {
                var groupPermission=await groupPerMissionRepository.GetAllGroupPermission(search, page,pageSize);
                return Ok(Repository<List<GroupPermissionDTO>>.WithData(groupPermission,200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{Id}")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetGroupPermission(string Id)
        {
            try
            {
                var groupPermission = await groupPerMissionRepository.GetGroupPermission(Id);
                if(groupPermission==null)
                {
                    return NotFound();
                }
                return Ok(Repository<GroupPermissionDTO>.WithData(groupPermission, 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateGroupPermission(GroupPermissionModel model)
        {
            try
            {
                var result = await groupPerMissionRepository.CreateGroupPermission(model);
                if (!result.Succeeded)
                {
                    return BadRequest(Repository<string>.WithMessage("Tạo nhóm không thành công",400));
                }
                return Ok(Repository<string>.WithMessage("Tạo nhóm thành công", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{Id}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteGroupPermission(string Id)
        {
            try
            {
                var groupPermission = await groupPerMissionRepository.GetGroupPermission(Id);
                
                if (groupPermission==null)
                {
                    return NotFound();
                }
                await groupPerMissionRepository.DeleteGroupPermission(Id);
                return Ok(Repository<string>.WithMessage("Xóa nhóm thành công", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{Id}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateGroupPermission(string Id,GroupPermissionModel model)
        {
            try
            {
                var groupPermission = await groupPerMissionRepository.GetGroupPermission(Id);

                if (groupPermission == null)
                {
                    return NotFound();
                }
                var result= await groupPerMissionRepository.UpdateGroupPermission(Id,model);
                if (result.Succeeded)
                {
                    return Ok(Repository<string>.WithMessage("Cập nhập nhóm thành công", 200));
                }
                return BadRequest(Repository<string>.WithMessage("Cập nhập nhóm thất bại", 400));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
