using Flight.Authorize.DocumentAuthorize;
using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.DocumentFlightRepository;
using Flight.Repository.FlightRepository;
using Flight.Service.FileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentFlightController : ControllerBase
    {
        private readonly IDocumentFlightRepository documentFlightRepository;
        private readonly IFlightRepository flightRepository;
        private readonly IDocumentAuthorize documentAuthorize;

        public DocumentFlightController(IDocumentFlightRepository documentFlightRepository,IFlightRepository flightRepository,IDocumentAuthorize documentAuthorize) 
        {
            this.documentFlightRepository = documentFlightRepository;
            this.flightRepository = flightRepository;
            this.documentAuthorize = documentAuthorize;


        }
        [HttpPost]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateDocumentFlight([FromForm]DocumentFlightModel model)
        {
            try
            {
                var flight = await flightRepository.GetFlight(model.FlightId);
                if(flight!=null&& flight.IsConfirm==true)
                {
                    return BadRequest(Repository<string>.WithMessage("Không cho tạo tài liệu mới do chuyến bay đã kết thúc",200));
                }
                await documentFlightRepository.CreateDocumentFlightOriginal(model);
                return Ok(Repository<string>.WithMessage("Tạo tài liệu thành công",200));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
              
            
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteDocumentFlight(int id)
        {
            try
            {
                var documentFlight = await documentFlightRepository.GetDocumentFlight(id);
                if(documentFlight==null)
                {
                    return NotFound();
                }
                var flight = await flightRepository.GetFlight(documentFlight.FlightId);
                if(flight!=null&& flight.IsConfirm==true)
                {
                    return BadRequest(Repository<string>.WithMessage("Không cho xóa tài liệu mới do chuyến bay đã kết thúc",200));
                }
                await documentFlightRepository.DeleteDocumentFlight(id);
                return Ok(Repository<string>.WithMessage("Xóa tài liệu thành công", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetDocumentFlight(int id)
        {
            try
            {
                var documentFlight = await documentFlightRepository.GetDocumentFlight(id);
                if (documentFlight == null)
                {
                    return NotFound();
                }
                var result= await documentAuthorize.ReadDocumentAuthorize(id);
                if(result!=true)
                {
                    return Forbid();
                }
                return Ok(Repository<DocumentFlightDTO>.WithData(documentFlight, 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }
        [HttpGet("LastVersion/{flightId}")]
        [Authorize]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetAllDocumentFlightLastVersion(int flightId)
        {
            try
            {
                var documentFlight = await documentFlightRepository.GetAllDocumentFlightLastVersion(flightId);
                return Ok(Repository<List<ListDocumentFlight>?>.WithData(documentFlight, 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }
        [HttpGet("getAll")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetAllDocumentFlight(string?search,DateTime? date,int? documentTypeId,int? flightId, bool? isOrigin)
        {
            try
            {
                var documentFlight = await documentFlightRepository.GetAllDocumentFlight(flightId, isOrigin,search, documentTypeId, date);
                return Ok(Repository<List<ListDocumentFlight>?>.WithData(documentFlight, 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }
        [HttpPost("download")]
        [Authorize]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> downlaodFile(List<int> ids)
        {
            try
            {
                foreach(int id in ids)
                {
                    bool results = await documentAuthorize.ReadDocumentAuthorize(id);
                    if (!results)
                    {
                        return Forbid();
                    }
                }
                var result=await documentFlightRepository.DownloadFile(ids);
                return File(result.Item1, result.Item2, result.Item3);
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPut("{id}")]
        [Authorize]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> UpdateDocumentFlight(int id, DocumentFlightModel model)
        {
            try
            {

                var documentFlight = await documentFlightRepository.GetDocumentFlight(id);
                if (documentFlight == null)
                {
                    return NotFound();
                }
                var flight = await flightRepository.GetFlight(model.FlightId);
                if(flight!=null&& flight.IsConfirm==true)
                {
                    return BadRequest(Repository<string>.WithMessage("Không cho cập nhập tài liệu mới do chuyến bay đã kết thúc",200));
                }
                await documentFlightRepository.UpdateDocumentFlight(id, model);
                return Ok(Repository<string>.WithMessage("Cập nhập tài liệu thành công", 200));
            }
            catch
            {
                return BadRequest();
            }
            
        }
        [HttpDelete("documentUser/{id}")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> DeleteDocumnetFlightUser(int id)
        {
            try
            {

                var documentFlight = await documentFlightRepository.GetDocumentFlight(id);
                if (documentFlight == null)
                {
                    return NotFound();
                }
                if(documentFlight.Version==VersionModel.Origin)
                {
                    return BadRequest(Repository<string>.WithMessage("Không xóa được do là tài liệu gốc", 200));
                }
                var flight = await flightRepository.GetFlight(documentFlight.FlightId);
                if(flight!=null&& flight.IsConfirm==true)
                {
                    return BadRequest(Repository<string>.WithMessage("Không cho xóa tài liệu mới do chuyến bay đã kết thúc",200));
                }
                await documentFlightRepository.DeleteDocumnetFlightUser(id);
                return Ok(Repository<string>.WithMessage("Xóa tài liệu thành công", 200));
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPost("User")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> CreateDocumnetFlightUser([FromForm]DocumentFlightUserModel model)
        {
            try
            {
                var flight = await flightRepository.GetFlight(model.FlightId);
                if (flight != null && flight.IsConfirm == true)
                {
                    return BadRequest(Repository<string>.WithMessage("Không cho tạo tài liệu mới do chuyến bay đã kết thúc", 200));
                }
                await documentFlightRepository.CreateDocumentFlightUser(model);
                return Ok(Repository<string>.WithMessage("tạo tài liệu thành công", 200));
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPut("User/{id}")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> EditDocumnetFlightUser(int id,[FromForm]DocumentFlightUpdateUserModel model)
        {
            try
            {
                var documentFlight=await documentFlightRepository.GetDocumentFlight(id);
                if (documentFlight == null)
                {
                    return NotFound();
                }
                var result= await documentAuthorize.EditDocumentAuthorize(id);
                if(!result)
                {
                    return Forbid();
                }
                var flight = await flightRepository.GetFlight(documentFlight.FlightId);
                if(flight!=null&& flight.IsConfirm==true)
                {
                    return BadRequest(Repository<string>.WithMessage("Không cho cập nhập tài liệu mới do chuyến bay đã kết thúc",200));
                }
                await documentFlightRepository.EditDocumentFlightUser(id, model);
                return Ok(Repository<string>.WithMessage("tạo tài liệu thành công", 200));
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
