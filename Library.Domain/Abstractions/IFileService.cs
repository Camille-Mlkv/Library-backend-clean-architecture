namespace Library.Domain.Abstractions
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(byte[] fileData, string fileName);
        void DeleteFileAsync(string fileName);
    }
}
