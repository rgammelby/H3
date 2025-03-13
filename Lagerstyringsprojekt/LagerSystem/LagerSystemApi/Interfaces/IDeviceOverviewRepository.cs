using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface IDeviceOverviewRepository
    {
        Task<DeviceOverview?> GetDeviceOverviewById(int id);
        Task<DeviceOverview?> GetDeviceOverviewByModelAndType(string model, int deviceType);
        Task<List<DeviceOverview>> GetAllDeviceOverviews();
        Task<DeviceOverview> AddDeviceOverview(DeviceOverview deviceOverview);
        Task<DeviceOverview> UpdateDeviceOverview(DeviceOverview deviceOverview);
        Task<DeviceType> GetDeviceTypeById(int id);
        Task<bool> UpdateDeviceQuantity(int id, int quantity);
        Task<bool> DecrementAvailableQuantity(int id);
    }
}
