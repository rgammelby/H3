namespace LagerSystemApi.Interfaces
{
    public interface IUploadImages
    {
        Task<string> SaveImage(IFormFile file);
        void DeleteImage(string path);
    }
}
