namespace LagerSystemApi.Interfaces
{
    public interface IUploadImages
    {
        Task<string> SaveImage(IFormFile file);
    }
}
