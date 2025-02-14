namespace LagerSystemApi.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<LocationCupboard>> GetAllCupboards();
        Task<List<LocationRoom>> GetAllRooms();
    }
}
