using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface IDeviceService
    {
        Task<List<DeviceDTO>> GetAllDevices();   // Fetch all devices (DTO)
        Task<List<DeviceDTO>> GetSingleDevicesByModel(string model);
        Task<DeviceDTO?> GetDevice(int id);    // Fetch a single device (DTO)
        Task<DeviceDTO?> AddDevice(AddSingleDeviceDTO device);
        Task<DeviceDTO?> UpdateDevice(int id, UpdateDeviceDTO update);
        Task<DeviceDTO?> DeactivateDevice(int id);
        
    }
}
