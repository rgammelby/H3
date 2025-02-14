using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface ILocationService
    {
        Task<List<LocationCupboardDTO>> GetAllCupboards();
        Task<List<LocationRoomDTO>> GetAllRooms();
        Task<LocationCupboardDTO> GetCupboardById(int id);
        Task<LocationRoomDTO> GetRoomById(int id);
    }
}
