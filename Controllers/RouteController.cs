using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.RouteRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteRepository routeRepository;

        public RouteController(IRouteRepository routeRepository) 
        { 
            this.routeRepository = routeRepository;
        }
        [HttpGet]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetAllRoute() 
        {
            try
            {
                var route =await routeRepository.GetAllRoute();
                return Ok(Repository<List<RouteDTO>>.WithData(route,200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{Id}")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetRoute(int Id)
        {
            try
            {
                var route = await routeRepository.GetRoute(Id);
                if(route == null)
                {
                    return NotFound();
                }
                return Ok(Repository<RouteDTO?>.WithData(route, 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> CreateRoute(RouteModel model)
        {
            try
            {
                await routeRepository.CreateRoute(model);
                return Ok(Repository<string>.WithMessage("Tạo thành công tuyến đường", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{Id}")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> DeleteRoute(int Id)
        {
            try
            {
                var route=await routeRepository.GetRoute(Id);
                if(route==null)
                {
                    return NotFound();
                }
                await routeRepository.DeleteRoute(Id);
                return Ok(Repository<string>.WithMessage("Xóa thành công tuyến đường", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{Id}")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> UpdateRoute(int Id,RouteModel model)
        {
            try
            {
                var route = await routeRepository.GetRoute(Id);
                if (route == null)
                {
                    return NotFound();
                }
                await routeRepository.UpdateRoute(Id,model);
                return Ok(Repository<string>.WithMessage("Cập nhập thành công tuyến đường", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
