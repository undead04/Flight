using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Flight.Service.ReadTokenService;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Flight.Repository.DocumentFlightRepository
{
    public class DocumentFLightRepository : IDocumentFlightRepository
    {
        private readonly IReadTokenService readTokenService;
        private readonly MyDbContext context;

        public DocumentFLightRepository(MyDbContext context,IReadTokenService readTokenService) 
        {
            this.readTokenService=readTokenService;
            this.context = context;
        }   
        public async Task CreateDocumentFlightOriginal(DocumentFileOriginalModel model)
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
            await context.documentFlight.AddAsync(documentFlight);
            await context.SaveChangesAsync();
        }

        public async Task DeleteDocumentFlight(int id)
        {
            var documentFlight = await context.documentFlight.FirstOrDefaultAsync(doc=>doc.Id==id);
            if(documentFlight!=null)
            {
                context.Remove(documentFlight);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<ListDocumentFlight>> GetAllDocumentFlight(int flightId, bool? IsOrigin)
        {
            var documentFlight = await context.documentFlight.Include(f=>f.DocumentType).Where(doc => doc.FlightId == flightId).ToListAsync();
            if (IsOrigin.HasValue)
            {
                documentFlight = documentFlight.Where(doc => doc.Version == VersionModel.Origin).ToList();
            }
            return documentFlight.Select(doc => new ListDocumentFlight
            {
                Id=doc.Id,
                Name=doc.Name,
                DocumentType=doc.DocumentType!.Name,
                CreateDate=doc.Create_Date,
                Creator=doc.ApplicationUser!.UserName,
                Lastversion=doc.Version
            }).ToList();
        }


        public async Task<List<ListDocumentFlight>> GetAllDocumentFlightLastVersion(int flightId)
        {
            var documentFlight = await context.documentFlight.Include(f=>f.ApplicationUser)
                .Include(f=>f.DocumentType)
                .Where(doc => doc.FlightId == flightId).ToListAsync();
            var lastDocumentFlight = documentFlight
                    .GroupBy(doc => doc)
                    .Select(bo => new
                    {
                        Document = bo.Key,
                        MaxVersion = bo.Max(doc => Convert.ToInt32(doc.Version)),
                    });
            return lastDocumentFlight.Select(doc => new ListDocumentFlight
            {
                Id=doc.Document.Id,
                Name=doc.Document.Name,
                DocumentType=doc.Document.DocumentType!.Name,
                CreateDate=doc.Document.Create_Date,
                Creator=doc.Document.ApplicationUser!.UserName,
                Lastversion=doc.MaxVersion.ToString(),
            }).ToList();
                  


        }

        public async Task<DocumentFlightDTO?> GetDocumentFlight(int id)
        {
            var documnetFlights = await context.documentFlight.
                Include(f => f.DocumentType)
                .Include(f => f.ApplicationUser)
                .Include(f => f.Flight)
                .FirstOrDefaultAsync(doc => doc.Id == id);
            if(documnetFlights == null)
            {
                return null;
            }
            return new DocumentFlightDTO
            {
                Id = documnetFlights.Id,
                DocumentType = documnetFlights.DocumentType!.Name,
                CreateDate = documnetFlights.Create_Date,
                Creator = documnetFlights.ApplicationUser!.UserName,
                FlightNo = documnetFlights.Flight!.FlightNo,
                Name = documnetFlights.Name,
                Lastversion = documnetFlights.Version,
                Permission = documnetFlights.DocumentFlightPermissions!.Select(e => new { e.GroupPermission!.Name, e.DocumentFlight!.DocumentType!.PermissionDocuments.FirstOrDefault(doc => doc.ClaimsType == e.GroupPermission!.Name)!.ClaimsValue }).ToDictionary(x => $"{x.Name}", x => x.ClaimsValue)
            };
        }

        public Task UpdateDocumentFlight()
        {
            throw new NotImplementedException();
        }
    }
}
