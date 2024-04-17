using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Repository.DocumentFlightRepository
{
    public interface IDocumentFlightRepository
    {
        Task CreateDocumentFlightOriginal(DocumentFlightModel model);
        Task<List<ListDocumentFlight>> GetAllDocumentFlight(int? flightId,bool? IsOrigin, string? search, int? categoryId, DateTime? date);
        Task<List<ListDocumentFlight>> GetAllDocumentFlightLastVersion(int flightId);
        Task<DocumentFlightDTO?> GetDocumentFlight(int id);
        Task UpdateDocumentFlight(int id, DocumentFlightModel model);
        Task DeleteDocumentFlight(int id);
        Task<(byte[], string, string)> DownloadFile(List<int> documentFlightId);
        Task CreateDocumentFlightUser(DocumentFlightUserModel model);
        Task EditDocumentFlightUser(int id, DocumentFlightUpdateUserModel model);
        Task DeleteDocumnetFlightUser(int id);
    }
}
