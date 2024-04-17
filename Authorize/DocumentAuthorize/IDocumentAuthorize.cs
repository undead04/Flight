namespace Flight.Authorize.DocumentAuthorize
{
    public interface IDocumentAuthorize
    {
        Task<bool> ReadDocumentAuthorize(int id);
        Task<bool> EditDocumentAuthorize(int id);
    }
}
