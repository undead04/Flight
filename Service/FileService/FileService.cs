using Microsoft.Extensions.Hosting;
using System;

namespace Flight.Service.FileService
{
    public class FileService:IFileService
    {
        private readonly IWebHostEnvironment environment;

        public FileService(IWebHostEnvironment environment) 
        {
            this.environment = environment;
        }
        public string GetFilePath(string ProCode,string version)
        {
            return environment.WebRootPath + $"\\Uploads\\{ProCode}\\{version}";
        }

        public async Task<string> UploadFile(string version, string procode, IFormFile image)
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
        public string GetUrlFile(string Procode, string imageName,string version)
        {
            string hostUrl = "https://localhost:7169/";
            return hostUrl + $"Uploads/{Procode}/{version}" + imageName;
        }
        public void DeleteFile(string ProCode,string version, string nameImage)
        {
            var filePath = GetFilePath(ProCode,version);
            var imagePath = filePath + $"\\{nameImage}";
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}
