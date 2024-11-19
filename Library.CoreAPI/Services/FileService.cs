namespace Library.CoreAPI.Services
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

        public async Task<string> SaveFileAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "default-book.png";
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            var filePath = Path.Combine(_imageFolderPath, fileName);

            Directory.CreateDirectory(_imageFolderPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
