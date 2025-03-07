namespace LagerSystemApi.Interfaces
{
    public interface IUploadImages
    {
        Task<string> SaveImage(IFormFile file);
        Task<bool> DeleteImage(string file_path);
    }
}
