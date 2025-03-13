using LagerSystemApi.Models.DTO;
using LagerSystemApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;
using LagerSystemApi.Interfaces;

namespace LagerSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error getting device by id: {id}");
            }
        }

        [HttpGet("GetAllDevices")]
        public async Task<IActionResult> GetAllDevices()
        {
            try
            {
                var devices = await _deviceService.GetAllDevices();
                if (devices == null)
                {
                    return BadRequest(new { message = "NoDevice found." });
                }
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error getting all devices");
            }
        }

        [HttpPost("AddDevice")]
        public async Task<IActionResult> AddNewDevice([FromBody] AddSingleDeviceDTO addSingleDeviceDTO)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error adding new deivce");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] UpdateDeviceDTO updateDeviceDTO)
        {
            try
            {
                if (updateDeviceDTO == null || id <= 0)
                {
                    return BadRequest(new { message = "Invalid device data." });
                }

                var updatedDevice = await _deviceService.UpdateDevice(id, updateDeviceDTO);

                if (updatedDevice == null)
                {
                    return NotFound(new { message = $"Device with id {id} not found or update failed." });
                }

                return Ok(updatedDevice);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error updating device with id: {id}");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> DeactivateDevice(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error deactivating deivce with id: {id}");
            }
        }

        [HttpGet("{model}")]
        public async Task<IActionResult> GetSingleDevicesByModel(string model)
        {
            Console.WriteLine($"Origin: {Request.Headers["Origin"]}");
            try
            {
                if (string.IsNullOrEmpty(model)) return BadRequest(new { message = "No models corresponding with search term. " });
                var devices = await _deviceService.GetSingleDevicesByModel(model);

                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error getting devices by device model: {model}");
            }
        }
    }
}
