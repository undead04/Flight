using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Repository.DocumentFlightPermissionRepository
{
    public interface IDocumenttFlightPermissionRepository
    {
        Task CreateDocumentFlightPermission(DocumentFlightPermissionModel model);
        Task DeleteDocumentFlightPermission(int DocumentFlightId);
        Task<List<DocumentFlightPermissionDTO>?> GetAllDocumentFlightPermission(int DocumentFlightId);
        
    }
}
