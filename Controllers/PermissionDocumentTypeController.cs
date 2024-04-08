using Flight.Model.DTO;
using Flight.Repository.PermissionRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionDocumentTypeController : ControllerBase
    {
        private readonly IPermissionRepository permissionRepository;

        public PermissionDocumentTypeController(IPermissionRepository permissionRepository) 
        {
            this.permissionRepository = permissionRepository;
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetPermissionDocumentType(int Id)
        {
            try
            {
                var permissionDocumentType = await permissionRepository.GetPermission(Id);
                if (permissionDocumentType == null)
                {
                    return NotFound();
                }
                return Ok(Repository<List<PermissionDTO>>.WithData(permissionDocumentType, 200));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
