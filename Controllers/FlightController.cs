using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.FlightRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository flightRepository;

        public FlightController(IFlightRepository flightRepository)
        {
            this.flightRepository = flightRepository;
        }
        [HttpGet]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetAllFlight(string? search, DateTime? date, int? documentTypeId,int? page,int? pageSize)
        {
            try
            {
                var flights = await flightRepository.GetAllFlight(search, date, documentTypeId,page,pageSize);
                return Ok(Repository<List<FlightDTO>>.WithData(flights, 200));

            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{Id}")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetFlight(int Id)
        {
            try
            {
                var flights = await flightRepository.GetFlight(Id);
                if(flights!=null)
                {
                    return Ok(Repository<FlightDTO>.WithData(flights, 200));
                }
                return NotFound();
                

            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateFlight(FlightModel model)
        {
            try
            {
                var flights = await flightRepository.CreateFlight(model);
                if (flights!=0)
                {
                    return Ok(Repository<string>.WithMessage("Tạo chuyến bay thành công", 200));
                }
                return BadRequest(Repository<string>.WithMessage("Tạo chuyến bay thất bại", 400));


            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{Id}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateFlight(int Id,FlightModel model)
        {
            try
            {
                var flight = await flightRepository.GetFlight(Id);
                if (flight != null)
                {
                    await flightRepository.UpdateFlight(Id, model);
                    return Ok(Repository<string>.WithMessage("Cập nhật chuyến bay thành công", 200));
                }
                return NotFound();


            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{Id}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteFlight(int Id)
        {
            try
            {
                var flight = await flightRepository.GetFlight(Id);
                if (flight != null)
                {
                    await flightRepository.DeleteFlight(Id);
                    return Ok(Repository<string>.WithMessage("Xóa chuyến bay thành công", 200));
                }
                return NotFound();


            }
            catch
            {
                return BadRequest();
            }
        }
    }
}