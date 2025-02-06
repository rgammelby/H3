namespace LagerSystemApi.Repository
{
    public class DeviceOverviewRepository
    {
        public interface IDeviceOverviewRepository
        {
            Task<DeviceOverview?> GetDeviceOverviewById(int id);
            Task<List<DeviceOverview>> GetAllDeviceOverviews();
            Task AddDeviceOverview(DeviceOverview deviceOverview);
            Task UpdateDeviceOverview(DeviceOverview deviceOverview);

        }

        //public class DeviceOverviewRepository : IDeviceOverviewRepository
        //{
        //    private readonly Context _context;
        //    public DeviceOverviewRepository(Context db {
        //        _context = db;
        //    }

        //    public async Task<DeviceOverview> Get

        //}
    }
}
