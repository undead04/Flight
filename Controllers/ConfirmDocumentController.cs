using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.FlightRepository;
using Flight.Service.ConfirmDocumentService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmDocumentController : ControllerBase
    {
        private readonly IConfirmDocumentService confirmDocumentService;
        private readonly IFlightRepository flightRepository;

        public ConfirmDocumentController(IConfirmDocumentService confirmDocumentService,IFlightRepository flightRepository) 
        {
            this.confirmDocumentService = confirmDocumentService;
            this.flightRepository = flightRepository;
        }
        [HttpPut]
        public async Task<IActionResult> ConfirmDocument([FromForm]ConfirmDocumentModel model)
        {
            try
            {
                var flight = await flightRepository.GetFlight(model.FlightId);
                if(flight == null)
                {
                    return NotFound();
                }
                if(flight.IsConfirm!=false)
                {
                    return BadRequest(Repository<string>.WithMessage("Chuyến bay này đã confim rồi", 200));
                }
                await confirmDocumentService.ConfirmDocument(model);
                return Ok(Repository<string>.WithMessage("Thành công",200));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
