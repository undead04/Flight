using Flight.Model;

namespace Flight.Service.ConfirmDocumentService
{
    public interface IConfirmDocumentService
    {
        Task ConfirmDocument(ConfirmDocumentModel model);
    }
}
