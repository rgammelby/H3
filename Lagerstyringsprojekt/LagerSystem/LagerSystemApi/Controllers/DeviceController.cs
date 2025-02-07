using LagerSystemApi.Models.DTO;
using LagerSystemApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;

namespace LagerSystemApi.Controllers
{
    public interface IDeviceController
    {
        Task<IActionResult> GetDeviceById(int id);
        Task<IActionResult> GetAllDevices();
        Task<IActionResult> AddNewDevice(AddSingleDeviceDTO device);
        Task<IActionResult> UpdateDevice(int id, UpdateDeviceDTO device);
        Task<IActionResult> DeactivateDevice(int id);
    }

    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase, IDeviceController
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid Device id." });
            }

            var device = await _deviceService.GetDevice(id);
            if (device == null)
            {
                return NotFound(new { message = $"Device with id {id} not found." });
            }
            return Ok(device);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _deviceService.GetAllDevices();
            if (devices == null)
            {
                return BadRequest(new { message = "NoDevice found." });
            }
            return Ok(devices);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewDevice([FromBody] AddSingleDeviceDTO addSingleDeviceDTO)
        {
            if (addSingleDeviceDTO == null)
            {
                return BadRequest(new { message = "Invalid data." });
            }

            var createdDevice = await _deviceService.AddDevice(addSingleDeviceDTO);
            if (createdDevice == null)
            {
                return BadRequest(new { message = "Failed to create new device." });
            }

            return CreatedAtAction(nameof(GetDeviceById), new { id = createdDevice.id }, createdDevice);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] UpdateDeviceDTO updateDeviceDTO)
        {
            if (updateDeviceDTO == null || id <= 0)
            {
                return BadRequest(new { message = "Invalid device data." });
            }

            var updatedDevice = await _deviceService.UpdateDevice(updateDeviceDTO);

            if (updatedDevice == null)
            {
                return NotFound(new { message = $"Device with id {id} not found or update failed." });
            }

            return Ok(updatedDevice);

        }

        [HttpPatch]
        public async Task<IActionResult> DeactivateDevice(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid device data." });
            }

            var deactivatedDevice = await _deviceService.DeactivateDevice(id);

            if (deactivatedDevice == null)
            {
                return NotFound(new { message = $"Unable to deactivate device {id}." });
            }

            return Ok(deactivatedDevice);
        }
    }
}
