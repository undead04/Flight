using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Repository.DocumentFlightRepository
{
    public interface IDocumentFlightRepository
    {
        Task CreateDocumentFlightOriginal(DocumentFileOriginalModel model);
        Task<List<ListDocumentFlight>> GetAllDocumentFlight(int flightId,bool? IsOrigin);
        Task<List<ListDocumentFlight>> GetAllDocumentFlightLastVersion(int flightId);
        Task<DocumentFlightDTO?> GetDocumentFlight(int id);
        Task UpdateDocumentFlight();
        Task DeleteDocumentFlight(int id);
    }
}
