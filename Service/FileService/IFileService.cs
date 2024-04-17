namespace Flight.Service.FileService
{
    public interface IFileService
    {
        Task<string> UploadFile(string? version, string procode, IFormFile file);
        string GetFilePath(string ProCode,string? version);
        string GetUrlFile(string Procode, string fileName, string? version);
        void DeleteFile(string ProCode, string fileName, string? version);
        void RenameImage(string ProCode,string? version, string oldName, string newName);
        string GetExtensionFile(string ProCode, string name,string? version);
    }
}
