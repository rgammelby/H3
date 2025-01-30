using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;

namespace LagerSystemApi.Services
{
    public interface IDeviceService
    {
        Task<DeviceDTO[]> GetAll();
        Task<DeviceDTO> Get(int id);
        Task AddDevice(DeviceDTO device);
        Task UpdateDevice(UpdateDeviceDTO update);
        Task DeactivateDevice(int id);
    }
    public class DeviceService: IDeviceService
    {
        IDeviceRepository _device;
        public DeviceService(IDeviceRepository device)
        {
            _device = device;
        }

        public Task<DeviceDTO[]> GetAll()
        {
            return _device.GetAll();
        }
        public Task<DeviceDTO> Get(int id)
        {
            if (id == 0) return null;
            return _device.Get(id);
        }
        public async Task AddDevice(DeviceDTO device)
        {
            if (!HasNullProps(device)) await _device.Add(device);
        }
        public async Task UpdateDevice(UpdateDeviceDTO update)
        {
            if (!HasNullProps(update)) await _device.Update(update);
        }
        public async Task DeactivateDevice(int id)
        {
            if (id != 0) await _device.DeactivateDevice(id);
        }

        /*
         * These are private methods only used for validating device info
        */

        private bool HasNullProps(DeviceDTO device)
        {
            return device.id == null ||
                device.status == null ||
                device.is_archived == null ||
                device.name == null ||
                device.description == null ||
                device.qr == null ||
                device.location_id == null;
        }

        private bool HasNullProps(UpdateDeviceDTO device)
        {
            return device.id == null ||
                device.status == null ||
                device.is_archived == null ||
                device.name == null ||
                device.description == null ||
                device.qr == null ||
                device.location_id == null;
        }
    }
}
