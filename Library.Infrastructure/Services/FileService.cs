using Microsoft.Extensions.Configuration;

namespace Library.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _imageFolderPath;
        public FileService(IConfiguration configuration)
        {
            _imageFolderPath = configuration["ImageSettings:ImageFolderPath"] ?? "wwwroot/Images";
        }
        public void DeleteFileAsync(string fileName)
        {
            if (fileName == "default-book.png")
            {
                return;
            }

            var fullPath = Path.Combine(_imageFolderPath, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task<string> SaveFileAsync(byte[] fileData, string fileName)
        {
            if (fileData == null || fileData.Length == 0)
            {
                return "default-book.png";
            }

            var newFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
            var filePath = Path.Combine(_imageFolderPath, newFileName);

            Directory.CreateDirectory(_imageFolderPath);

            await File.WriteAllBytesAsync(filePath, fileData);

            return newFileName;
        }
    }
}
