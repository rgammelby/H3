namespace LagerSystemApi.Interfaces
{
    public interface IDeviceRepository
    {
        Task<SingleDevice?> GetDeviceById(int id); // Returns domain model
        Task<List<SingleDevice>> GetAllDevices();  // Returns list of domain models
        Task<SingleDevice> AddDevice(SingleDevice device);       // Accepts domain model for adding
        Task<SingleDevice> UpdateDevice(SingleDevice device);    // Updates a domain model

        Task<List<SingleDevice>> GetSingleDevicesByModel(string model);
    }
}
