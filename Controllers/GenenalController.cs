using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.GeneralRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenenalController : ControllerBase
    {
        private readonly IGeneralRepository generalRepository;

        public GenenalController(IGeneralRepository generalRepository) 
        {
            this.generalRepository = generalRepository;
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetGeneral(int id)
        {
            try
            {
                var general = await generalRepository.GetGeneral(id);
                if(general==null)
                {
                    return NotFound();
                }
                return Ok(Repository<GeneralDTO>.WithData(general, 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateGeneral([FromForm] GeneralModel model)
        {
            try
            {
                await generalRepository.CreateGeneral(model);
                
                return Ok(Repository<string>.WithMessage("Tao thành công", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateGerena(int id,[FromForm]GeneralModel model)
        {
            try
            {
                var general = await generalRepository.GetGeneral(id);
                if (general == null)
                {
                    return NotFound();
                }

                await generalRepository.UpDateGeneral(id,model);

                return Ok(Repository<string>.WithMessage("Cập nhập thành công", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
