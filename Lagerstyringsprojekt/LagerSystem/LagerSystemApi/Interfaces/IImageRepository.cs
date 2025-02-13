using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface IImageRepository
    {
        Task<string> GetImage(int id);
    }
}
