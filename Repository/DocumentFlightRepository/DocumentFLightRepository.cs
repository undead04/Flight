using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.DocumentFlightPermissionRepository;
using Flight.Service.FileService;
using Flight.Service.ReadTokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace Flight.Repository.DocumentFlightRepository
{
    public class DocumentFLightRepository : IDocumentFlightRepository
    {
        private readonly IReadTokenService readTokenService;
        private readonly MyDbContext context;
        private readonly IFileService fileService;
        private readonly IDocumenttFlightPermissionRepository documenttFlightPermissionRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public DocumentFLightRepository(MyDbContext context,IReadTokenService readTokenService,IFileService fileService
            ,IDocumenttFlightPermissionRepository documenttFlightPermissionRepository,UserManager<ApplicationUser> userManager) 
        {
            this.readTokenService=readTokenService;
            this.context = context;
            this.fileService = fileService;
            this.documenttFlightPermissionRepository = documenttFlightPermissionRepository;
            this.userManager = userManager;
        }   
        public async Task CreateDocumentFlightOriginal(DocumentFlightModel model)
        {
            string userId = await readTokenService.ReadJWT();
            var documentFlight = new DocumentFlight
            {
                Create_Date = DateTime.Now.Date,
                FlightId=model.FlightId,
                CreateUserId=userId,
                DocumentTypeId=model.DocumentTypeId,
                Name=model.documentFile!.FileName,
                Version=VersionModel.Origin,
            };
            await fileService.UploadFile(documentFlight.Version, "Document", model.documentFile);
            documentFlight.ExtensionFile = fileService.GetExtensionFile("Documnet", documentFlight.Name, documentFlight.Version);
            await context.documentFlight.AddAsync(documentFlight);
            await context.SaveChangesAsync();
            
            foreach(string groupPermissionId in model.GroupPermissionId)
            {
                var documentPermissionModel = new DocumentFlightPermissionModel
                {
                    DocumentFlightId = documentFlight.Id,
                    GroupPermissionId = groupPermissionId,
                };
                await documenttFlightPermissionRepository.CreateDocumentFlightPermission(documentPermissionModel);
            }
            
            
        }

        public async Task DeleteDocumentFlight(int id)
        {
            var documentFlight = await context.documentFlight.FirstOrDefaultAsync(doc=>doc.Id==id);
            if(documentFlight!=null)
            {

                await documenttFlightPermissionRepository.DeleteDocumentFlightPermission(documentFlight.Id);
                context.Remove(documentFlight);
                fileService.DeleteFile("Document",documentFlight.Name,documentFlight.Version);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<ListDocumentFlight>> GetAllDocumentFlight(int? flightId, bool? IsOrigin, string? search, int? categoryId, DateTime? date)
        {
            string userId = await readTokenService.ReadJWT();
            var user =await userManager.FindByIdAsync(userId);
            var role = await userManager.GetRolesAsync(user);
            string roleName = string.Join(string.Empty, role);
            var documentFlight = context.documentFlight     
                .Include(f => f.ApplicationUser)
                .Include(f => f.DocumentType)
                .ThenInclude(f=>f!.PermissionDocuments)
                .Include(f=>f.Flight)
                .Include(f=>f.DocumentFlightPermissions)!
                .ThenInclude(f=>f.GroupPermission)
                .AsQueryable();
            if (IsOrigin.HasValue)
            {
                documentFlight = documentFlight.Where(doc => doc.Version == VersionModel.Origin);
            }
            if(flightId.HasValue)
            {
                documentFlight = documentFlight.Where(doc => doc.FlightId == flightId.Value);
            }
            if(date.HasValue)
            {
                documentFlight = documentFlight.Where(doc => doc.Create_Date == date.Value);
            }
            if(!string.IsNullOrEmpty(search))
            {
                documentFlight = documentFlight.Where(doc => doc.Name.Contains(search));
            }
            if (categoryId.HasValue)
            {
                documentFlight=documentFlight.Where(doc=>doc.DocumentTypeId == categoryId.Value);
            }
            if(roleName!="admin")
            {
                documentFlight = documentFlight
                    .Where(doc => doc.DocumentFlightPermissions!.Any(doc => doc.GroupPermission!.Name == roleName));
                documentFlight = documentFlight.Where(doc=>doc.DocumentType!.PermissionDocuments!.Any(per=>per.ClaimsValue.Contains("read")));
            }
            return await documentFlight.Select(doc => new ListDocumentFlight
            {
                Id=doc.Id,
                FlightNo=doc.Flight!.FlightNo,
                Name=doc.Name,
                DocumentType=doc.DocumentType!.Name,
                CreateDate=doc.Create_Date,
                Creator=doc.ApplicationUser!.UserName,
                Lastversion=doc.Version,
                UrlFile = fileService.GetUrlFile("Document", doc.Name, doc.Version)
            }).ToListAsync();
        }


        public async Task<List<ListDocumentFlight>> GetAllDocumentFlightLastVersion(int flightId)
        {
            var documentFlight = await context.documentFlight
                .Include(f=>f.ApplicationUser)
                .Include(f=>f.DocumentType)
                .Where(doc => doc.FlightId == flightId).ToListAsync();
            var lastDocumentFlight = documentFlight
             .Select(doc => new
             {
                 Document = doc,
                 Version = doc.Version
             })
             .GroupBy(doc => doc.Document.Name)
             .Select(bo => bo.OrderByDescending(doc => doc.Version).First())
             .OrderByDescending(bo => bo.Version)
             .Select(bo => new
             {
                 Document = bo.Document,
             });

            return lastDocumentFlight.Select(doc => new ListDocumentFlight
            {
                Id=doc.Document.Id,
                Name=doc.Document.Name,
                DocumentType=doc.Document.DocumentType!.Name,
                CreateDate=doc.Document.Create_Date,
                Creator=doc.Document.ApplicationUser!.UserName,
                Lastversion=doc.Document.Version!,
                UrlFile = fileService.GetUrlFile("Document", doc.Document.Name, doc.Document.Version!)
            }).ToList();
                  
        }

        public async Task<DocumentFlightDTO?> GetDocumentFlight(int id)
        {
            var documentFlight = await context.documentFlight.
                Include(f => f.DocumentType)
                .Include(f => f.ApplicationUser)
                .Include(f => f.Flight)
                .FirstOrDefaultAsync(doc => doc.Id == id);
            if(documentFlight == null)
            {
                return null;
            }
            var updateVersion = await context.documentFlight.Where(doc => doc.Name == documentFlight.Name && doc.Version != documentFlight.Version).ToListAsync();
            
            return new DocumentFlightDTO
            {
                Id = documentFlight.Id,
                FlightId=documentFlight.FlightId,
                DocumentType = documentFlight.DocumentType!.Name,
                CreateDate = documentFlight.Create_Date,
                Creator = documentFlight.ApplicationUser!.UserName,
                FlightNo = documentFlight.Flight!.FlightNo,
                Name = documentFlight.Name,
                Version = documentFlight.Version,
                UrlFile = fileService.GetUrlFile("Document", documentFlight.Name, documentFlight.Version),
                Permission =await documenttFlightPermissionRepository.GetAllDocumentFlightPermission(documentFlight.Id),
                UpdateVersion=updateVersion.Select(up=>new ListDocumentFlight
                {
                    Id=up.Id,
                    DocumentType=up.DocumentType!.Name,
                    CreateDate=up.Create_Date.Date,
                    Creator=up.ApplicationUser!.UserName,
                    Name=up.Name,
                    Lastversion=up.Version,
                    UrlFile=fileService.GetUrlFile("Document",up.Name,up.Version)
                }).ToList()
            };
        }

        public async Task UpdateDocumentFlight(int id, DocumentFlightModel model)
        {
            var documentFlight = await context.documentFlight.FirstOrDefaultAsync(doc => doc.Id == id);
            if(documentFlight!=null)
            {
                documentFlight.DocumentTypeId = model.DocumentTypeId;
                foreach(var documentPermissionId in model.GroupPermissionId)
                {
                    var documentPermissionModel = new DocumentFlightPermissionModel
                    {
                        DocumentFlightId = documentFlight.Id,
                        GroupPermissionId = documentPermissionId,
                    };
                    await documenttFlightPermissionRepository.CreateDocumentFlightPermission(documentPermissionModel);
                }
                if(model.documentFile!=null)
                {
                    if(model.documentFile.Length>0)
                    {
                        await fileService.UploadFile(documentFlight.Version, "Document", model.documentFile);
                    }
                }
            }
        }
        public async Task<(byte[], string, string)> DownloadFile(List<int> documentFlightIds)
        {
            try
            {
                string zipFileName = $"flight_documents_{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip";
                string zipFilePath = Path.Combine(Path.GetTempPath(), zipFileName);
                List<string> _GetFilePath = new List<string>();
                foreach (int documentId in documentFlightIds)
                {
                    var documentFlight = await GetDocumentFlight(documentId);
                    var filePath = fileService.GetFilePath("Document", documentFlight!.Version) + $"\\{documentFlight.Name}";
                    _GetFilePath.Add(filePath);

                }
                using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                {
                    foreach (var documentPath in _GetFilePath)
                    {
                        zipArchive.CreateEntryFromFile(documentPath, Path.GetFileName(documentPath));
                    }
                }
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(zipFilePath, out var _ContentType))
                {
                    _ContentType = "application/octet-stream";
                }
                var _ReadAllBytesAsync = await File.ReadAllBytesAsync(zipFilePath);
                return (_ReadAllBytesAsync, _ContentType, Path.GetFileName(zipFilePath));


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task CreateDocumentFlightUser(DocumentFlightUserModel model)
        {
            var createUserId = await readTokenService.ReadJWT();
            var documentFlight = new DocumentFlight
            {
                Name=model.DocumentTitle,
                CreateUserId=createUserId,
                Create_Date=DateTime.Now.Date,
                DocumentTypeId=model.DocumetTypeId,
                FlightId=model.FlightId,
                Version="1.1",
            };
            await context.documentFlight.AddAsync(documentFlight);
            await context.SaveChangesAsync();
            await fileService.UploadFile(documentFlight.Version, "Document", model.DocumentFile!);
            documentFlight.ExtensionFile = fileService.GetExtensionFile("Document", model.DocumentFile!.Name, documentFlight.Version);
            await context.SaveChangesAsync();
            fileService.RenameImage("Document", documentFlight.Name, model.DocumentFile!.FileName, documentFlight.Name + $"{documentFlight.ExtensionFile}");

        }

        public async Task EditDocumentFlightUser(int id, DocumentFlightUpdateUserModel model)
        {
            var documentFlight = await context.documentFlight.FirstOrDefaultAsync(doc => doc.Id == id);
            var createUserId = await readTokenService.ReadJWT();
            if(documentFlight!=null)
            {
                double version = double.Parse(documentFlight.Version);
                var newDocumentFlight = new DocumentFlight
                {
                    Name = documentFlight.Name,
                    CreateUserId = createUserId,
                    Create_Date = DateTime.Now.Date,
                    DocumentTypeId = documentFlight.DocumentTypeId,
                    FlightId = documentFlight.FlightId,
                    Version = ((int)version + 0.1).ToString(),
                    
                };
                await context.documentFlight.AddAsync(newDocumentFlight);
                await context.SaveChangesAsync();
                foreach (var documentFlightPermission in documentFlight.DocumentFlightPermissions!)
                {
                    var documentPermissionModel = new DocumentFlightPermissionModel
                    {
                        DocumentFlightId = newDocumentFlight.Id,
                        GroupPermissionId = documentFlightPermission.GroupPermissionId,
                    };
                    await documenttFlightPermissionRepository.CreateDocumentFlightPermission(documentPermissionModel);
                }
                if (model.DocumentFile != null)
                {
                    await fileService.UploadFile(newDocumentFlight.Version, "Document", model.DocumentFile!);
                    documentFlight.ExtensionFile = fileService.GetExtensionFile("Document", documentFlight.Name, documentFlight.Version);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteDocumnetFlightUser(int id)
        {
            var documentFlight = await context.documentFlight.FirstOrDefaultAsync(doc => doc.Id == id);
            if(documentFlight!=null)
            {
                if(documentFlight.Version!=VersionModel.Origin)
                {
                    await documenttFlightPermissionRepository.DeleteDocumentFlightPermission(id);
                    context.Remove(documentFlight);
                    fileService.DeleteFile("Document", documentFlight.Name, documentFlight.Version);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
