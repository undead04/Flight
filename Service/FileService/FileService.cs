using Flight.Data;
using Flight.Repository.DocumentFlightRepository;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using System;
using System.IO.Compression;

namespace Flight.Service.FileService
{
    public class FileService:IFileService
    {
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor) 
        {
            this.environment = environment;
            _httpContextAccessor = httpContextAccessor;
           
        }
        public string GetFilePath(string ProCode,string? version)
        {
            if(string.IsNullOrEmpty(version))
            {
                return environment.WebRootPath + $"\\Uploads\\{ProCode}";
            }
            return environment.WebRootPath + $"\\Uploads\\{ProCode}\\{version}";
        }

        public async Task<string> UploadFile(string? version, string procode, IFormFile image)
        {
            var filePath = GetFilePath(procode,version);
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string imagePath = filePath + $"\\{image.FileName}";
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return imagePath;
        }
        public string GetUrlFile(string Procode, string imageName,string? version)
        {
            var request = _httpContextAccessor.HttpContext!.Request;
            var scheme = request.Scheme; // Lấy giao thức (http hoặc https)
            var host = request.Host.Value; // Lấy tên host

            string hostUrl = $"{scheme}://{host}/";
            if(string.IsNullOrEmpty(version))
            {
                return hostUrl + $"Uploads/{Procode}/" + imageName;
            }
            return hostUrl + $"Uploads/{Procode}/{version}/" + imageName;
        }
        public void DeleteFile(string ProCode, string nameImage, string? version)
        {
            var filePath = GetFilePath(ProCode,version);
            var imagePath = filePath + $"\\{nameImage}";
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }


        public string GetExtensionFile(string ProCode, string name, string? version)
        {
            string filePath = GetFilePath(ProCode, version)+$"{name}";
            return Path.GetExtension(filePath);
        }
        public void RenameImage(string ProCode,string? version, string oldName, string newName)
        {
            string filePath = GetFilePath(ProCode, version)+$"{oldName}";
            string filePathNew = GetFilePath(ProCode, version) + $"{newName}";
            if (File.Exists(filePath))
            {
                File.Move(filePath, filePathNew);
            }

        }
    }
}
