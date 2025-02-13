using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface IImageService
    {
        Task<(byte[] FileData, string ContentType)> GetImage(int id);
    }
}
