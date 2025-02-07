namespace LagerSystemApi.Interfaces
{
    public interface IDeviceOverviewRepository
    {
        Task<DeviceOverview?> GetDeviceOverviewById(int id);
        Task<List<DeviceOverview>> GetAllDeviceOverviews();
        Task AddDeviceOverview(DeviceOverview deviceOverview);
        Task UpdateDeviceOverview(DeviceOverview deviceOverview);
    }
}
