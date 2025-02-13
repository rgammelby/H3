using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface ILocationService
    {
        Task<List<LocationCupboardDTO>> GetAllCupboards();
        Task<List<LocationRoomDTO>> GetAllRooms();
    }
}
