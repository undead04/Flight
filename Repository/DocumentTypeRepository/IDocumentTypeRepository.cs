using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Repository.DocumentTypeRepository
{
    public interface IDocumentTypeRepository
    {
        Task<int> CreateDocumentType(DocumentTypeModel model);
        Task<int> UpdateDocumentType(int id,DocumentTypeModel model);
        Task<List<DocumentTypeDTO>> GetAllDocumentType(string?search,int? categoryId,DateTime? date);
        Task<DocumentTypeDTO?> GetDocumentType(int id);
        Task DeleteDocumentType(int id);
    }
}
