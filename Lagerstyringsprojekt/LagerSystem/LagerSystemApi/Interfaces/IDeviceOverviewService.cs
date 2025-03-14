using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface IDeviceOverviewService
    {
        Task<List<DeviceOverviewDTO>> GetAllDeviceOverviews();   // Fetch all deviceOverviews (DTO)
        Task<DeviceOverviewDTO?> GetDeviceOverviewById(int id);    // Fetch a deviceOverview (DTO)
        Task<DeviceOverviewDTO?> AddDeviceOverview(AddDeviceOverviewDTO addDeviceOverviewDto);
        Task<DeviceOverviewDTO?> UpdateDeviceOverview(int id, UpdateDeviceOverviewDTO updateDeviceOverviewDto);
        Task<bool> UpdateDeviceQuantity(int id, int quantity);
        Task<bool> DecrementAvailableQuantity(int id);
    }
}
