using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.DocumentTypeRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeRepository documentTypeRepository;

        public DocumentTypeController(IDocumentTypeRepository documentTypeRepository) 
        {
            this.documentTypeRepository = documentTypeRepository;
        }
        [HttpPost]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> CreateDocumentType(DocumentTypeModel model)
        {
            try
            {
                var documentType = await documentTypeRepository.CreateDocumentType(model);
                if(documentType==0)
                {
                    return BadRequest(Repository<string>.WithMessage("Tạo thể loại thất bại", 400));
                }
                return Ok(Repository<string>.WithMessage("Tạo thể loại thành công", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetAllDocumentType(string?search,DateTime?date,int? documentTypeId)
        {
            try
            {
                var documentType = await documentTypeRepository.GetAllDocumentType(search,documentTypeId,date);
                return Ok(Repository<List<DocumentTypeDTO>>.WithData(documentType, 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{Id}")]
        [Authorize(Policy = "RequireRole")]
        public async Task<IActionResult> GetDocumentType(int Id)
        {
            try
            {
                var documentType = await documentTypeRepository.GetDocumentType(Id);
                if(documentType==null)
                {
                    return NotFound();
                }
                return Ok(Repository<DocumentTypeDTO>.WithData(documentType, 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{Id}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteDocumentType(int Id)
        {
            try
            {
                var documentType = await documentTypeRepository.GetDocumentType(Id);
                if (documentType == null)
                {
                    return NotFound();
                }
                await documentTypeRepository.DeleteDocumentType(Id);
                return Ok(Repository<string>.WithMessage("Xóa thành công thể loại", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{Id}")]
        [Authorize(Policy = "RequireRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateDocumentType(int Id,DocumentTypeModel model)
        {
            try
            {
                var documentType = await documentTypeRepository.GetDocumentType(Id);
                if (documentType == null)
                {
                    return NotFound();
                }
                var result= await documentTypeRepository.UpdateDocumentType(Id,model);
                if(result==0)
                {
                    return BadRequest(Repository<string>.WithMessage("Cập nhập thể loại thất bại",400));
                }
                return Ok(Repository<string>.WithMessage("Cập nhập thành công thể loại", 200));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
