using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.Services;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Controllers
{
    public interface IDeviceController
    {
        Task<DeviceDTO[]> GetAll();
        Task<DeviceDTO> Get(int id);
        Task Add(AddSingleDeviceDTO device);
        Task Update(UpdateDeviceDTO device);
        Task Deactivate(int id);


    }
    public class DeviceController: IDeviceController
    {
        private IDeviceService _deviceService;
        public DeviceController(IDeviceService service)
        {
            _deviceService = service;
        }

        [HttpGet("GetDevice")]
        public async Task<DeviceDTO> Get(int id)
        {
            return await _deviceService.Get(id);
        }

        [HttpGet("GetAllDevices")]
        public async Task<DeviceDTO[]> GetAll()
        {
            return await _deviceService.GetAll();
        }

        [HttpPost("AddDevice")]
        public async Task Add(AddSingleDeviceDTO device)
        {
            await _deviceService.AddDevice(device);
        }

        [HttpPut("UpdateDevice")]
        public async Task Update(UpdateDeviceDTO device)
        {
            await _deviceService.UpdateDevice(device);
        }

        [HttpPut("DeactivateDevice")]
        public async Task Deactivate(int id)
        {
            await _deviceService.DeactivateDevice(id);
        }
    }
}
