using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;
using Microsoft.IdentityModel.Tokens;

namespace LagerSystemApi.Services
{
    public interface IDeviceService
    {
        Task<DeviceDTO[]> GetAll();
        Task<DeviceDTO> Get(int id);
        Task AddDevice(AddSingleDeviceDTO device);
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
        public async Task AddDevice(AddSingleDeviceDTO device)
        {
            if (!HasNullProps(device))
            {
                await _device.Add(device);
            }
        }
        public async Task UpdateDevice(UpdateDeviceDTO update)
        {
            if (!HasNullProps(update))
            {
                await _device.Update(update);
            }
        }
        public async Task DeactivateDevice(int id)
        {
            if (id != 0)
            {
                await _device.DeactivateDevice(id);
            }
        }

        /*
         * These are private methods only used for validating device info
        */

        private bool HasNullProps(AddSingleDeviceDTO device)
        {
            return device.available_qty == 0 ||
                device.qty == 0 ||
                string.IsNullOrEmpty(device.image) ||
                device.status_id == 0 ||
                string.IsNullOrEmpty(device.description) ||
                string.IsNullOrEmpty(device.qr) ||
                device.location_id == 0;
        }

        private bool HasNullProps(UpdateDeviceDTO device)
        {
            return device.id == 0 ||
                device.status == 0 ||
                string.IsNullOrEmpty(device.description) ||
                string.IsNullOrEmpty(device.qr) ||
                device.location_id == 0;
        }
    }
}
