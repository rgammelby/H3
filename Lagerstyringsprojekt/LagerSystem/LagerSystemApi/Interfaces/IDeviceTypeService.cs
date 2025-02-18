using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface IDeviceTypeService
    {
        Task<List<DeviceTypeDTO>> GetAllDeviceTypes();
    }
}
