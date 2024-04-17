using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Flight.Repository.DocumentFlightRepository;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Flight.Repository.DocumentFlightPermissionRepository
{
    public class DocumentFlightPermissionRepository : IDocumenttFlightPermissionRepository
    {
        private readonly MyDbContext context;

        public DocumentFlightPermissionRepository(MyDbContext context) 
        {
            this.context = context;
        }
        public async Task CreateDocumentFlightPermission(DocumentFlightPermissionModel model)
        {
            var documentFlightPermission = new DocumentFlightPermission
            {
                DocumentFlightId = model.DocumentFlightId,
                GroupPermissionId = model.GroupPermissionId,
            };
            await context.documentFlightPermissions.AddAsync(documentFlightPermission);
            await context.SaveChangesAsync();

        }

        public async Task DeleteDocumentFlightPermission(int documentFlightId)
        {
            var documentFlightPermission = await context.documentFlightPermissions.Where(doc => doc.DocumentFlightId == documentFlightId).ToListAsync();
            if(documentFlightPermission.Count>0)
            {
                context.documentFlightPermissions.RemoveRange(documentFlightPermission );
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<DocumentFlightPermissionDTO>?> GetAllDocumentFlightPermission(int DocumentFlightId)
        {
            var documentFlightPermission = await context.documentFlightPermissions
                .Include(f=>f.GroupPermission)
                .Include(f=>f.DocumentFlight)
                .ThenInclude(f=>f!.DocumentType)
                .ThenInclude(f=>f!.PermissionDocuments)
                .Where(doc => doc.DocumentFlightId == DocumentFlightId).ToListAsync();
            if(documentFlightPermission != null)
            {
                return documentFlightPermission.Select(doc => new DocumentFlightPermissionDTO
                {
                    Name=doc.GroupPermission!.Name,
                    Permission=doc.DocumentFlight!.DocumentType!.PermissionDocuments!.FirstOrDefault(per=>per.GroupPermissionId==doc.GroupPermissionId)!.ClaimsValue.ToString(),
                }).ToList();
            }
            return null;
        }
        
    }
}
