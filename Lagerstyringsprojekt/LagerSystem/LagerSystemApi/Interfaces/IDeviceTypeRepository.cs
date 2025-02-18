namespace LagerSystemApi.Interfaces
{
    public interface IDeviceTypeRepository
    {
        Task<List<DeviceType>> GetAllDeviceTypes();
    }
}
