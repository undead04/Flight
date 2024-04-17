using Flight.Model.DTO;
using Flight.Service.ChangeOwnerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeOwnerSystemController : ControllerBase
    {
        private readonly IChangeOwnerService changeOwnerService;

        public ChangeOwnerSystemController(IChangeOwnerService changeOwnerService) 
        {
            this.changeOwnerService = changeOwnerService;
        }
        [HttpPut]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeOwnerSystem(string userId)
        {
            try
            {
                await changeOwnerService.ChangeOwerSystem(userId);
                return Ok(Repository<string>.WithMessage("đổi thành công", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ConfirmChangleSystem(string userId,string password)
        {
            try
            {
                await changeOwnerService.ComfimChangOwerSystem(userId, password);
                return Ok(Repository<string>.WithMessage("xác thực thành công thành công", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
