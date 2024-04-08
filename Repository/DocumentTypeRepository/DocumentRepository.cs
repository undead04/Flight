using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.GroupPermissionRepository;
using Flight.Repository.PermissionRepository;
using Flight.Service.ReadTokenService;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Flight.Repository.DocumentTypeRepository
{
    public class DocumentRepository : IDocumentTypeRepository
    {
        private readonly MyDbContext context;
        private readonly IPermissionRepository permissionRepository;
        private readonly IGroupPermissionRepository groupPermissionRepository;
        private readonly IReadTokenService readTokenService;

        public DocumentRepository(MyDbContext context,IReadTokenService readTokenService,
            IPermissionRepository permissionRepository,IGroupPermissionRepository groupPermissionRepository) 
        { 
            this.context=context;
            this.permissionRepository = permissionRepository;
            this.groupPermissionRepository = groupPermissionRepository;
            this.readTokenService = readTokenService;
        }
        public async Task<int> CreateDocumentType(DocumentTypeModel model)
        {
            string createUserId =await readTokenService.ReadJWT();
            var documentType = new DocumentType
            {
                Name = model.Name,
                Note=model.Note,
                Create_Date=DateTime.Now.Date,
                UserId=createUserId,
            };
            await context.documentTypes.AddAsync(documentType);
            await context.SaveChangesAsync();
            foreach(var permission in model.Permission!)
            {
                var nameGroup =await groupPermissionRepository.GetGroupPermission(permission.GroupPermissionId);
                var permissionModel = new PermissionModel
                {
                    GroupPermissionId = permission.GroupPermissionId,
                    DocumentTypeId=documentType.Id,
                    ClaimsType=nameGroup!.Name,
                    ClaimsValue=permission.ClaimsValue
                };
                await permissionRepository.CreatePermission(permissionModel);
                await context.SaveChangesAsync();
            }
            return documentType.Id;
        }

        public async Task DeleteDocumentType(int id)
        {
            var documentType = await context.documentTypes.FirstOrDefaultAsync(doc => doc.Id == id);
            if(documentType!=null)
            {
                await permissionRepository.DeletePermission(id);
                context.documentTypes.Remove(documentType);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<DocumentTypeDTO>> GetAllDocumentType(string? search, int? categoryId, DateTime? date)
        {
            var documentTypes = context.documentTypes.Include(f=>f.DocumentFlights).Include(f=>f.PermissionDocuments).Include(f=>f.ApplicationUser).AsQueryable();
            if(!string.IsNullOrEmpty(search))
            {
                documentTypes=documentTypes.Where(doc=>doc.DocumentFlights!=null?doc.DocumentFlights.Any(doc=>doc.Name.Contains(search)):false);
            }
            if(categoryId.HasValue)
            {
                documentTypes = documentTypes.Where(doc => doc.Id == categoryId.Value);
            }
            if(date.HasValue)
            {
                documentTypes = documentTypes.Where(doc => doc.Create_Date == date.Value.Date);
            }
            return await documentTypes.Select(doc => new DocumentTypeDTO
            {
                Id=doc.Id,
                Name=doc.Name,
                Create_At=doc.Create_Date,
                Creator=doc.ApplicationUser!.UserName,
                Permission=doc.PermissionDocuments!=null? doc.PermissionDocuments.Count:0
            }).ToListAsync();
        }

        public async Task<DocumentTypeDTO?> GetDocumentType(int id)
        {
            var documentType = await context.documentTypes.Include(f=>f.ApplicationUser).Include(f=>f.PermissionDocuments).FirstOrDefaultAsync(doc => doc.Id == id);
            if (documentType == null)
            {
                return null;
            }
            return new DocumentTypeDTO
            {
                Id = documentType.Id,
                Name = documentType.Name,
                Note = documentType.Note,
                Create_At = documentType.Create_Date,
                Creator = documentType.ApplicationUser!.UserName,
                Permission = documentType.PermissionDocuments != null ? documentType.PermissionDocuments.Count : 0,
            };

        }

        public async Task<int> UpdateDocumentType(int id, DocumentTypeModel model)
        {
            var documnetType = await context.documentTypes.FirstOrDefaultAsync(doc => doc.Id == id);
            if (documnetType == null)
            {
                return 0;
            }
            documnetType.Name = model.Name;
            documnetType.Note = model.Note;
            await permissionRepository.DeletePermission(id);
            foreach (var permission in model.Permission!)
            {
                var nameGroup = await groupPermissionRepository.GetGroupPermission(permission.GroupPermissionId);
                var permissionModel = new PermissionModel
                {
                    GroupPermissionId = permission.GroupPermissionId,
                    DocumentTypeId =documnetType.Id,
                    ClaimsType = nameGroup!.Name,
                    ClaimsValue = permission.ClaimsValue
                };
                await permissionRepository.CreatePermission(permissionModel);
            }
            await context.SaveChangesAsync();
            return documnetType.Id;
        }
    }
}
