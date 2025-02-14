namespace LagerSystemApi.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<LocationCupboard>> GetAllCupboards();
        Task<List<LocationRoom>> GetAllRooms(); 
        Task<LocationCupboard> GetCupboardById(int id);
        Task<LocationRoom> GetRoomById(int id);
    }
}
