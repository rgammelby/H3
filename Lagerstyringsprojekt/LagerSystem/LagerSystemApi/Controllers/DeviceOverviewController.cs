using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceOverviewController : ControllerBase
    {
        private readonly IDeviceOverviewService _deviceOverviewService;
        private readonly ILogger<DeviceOverviewController> _logger;

        public DeviceOverviewController(IDeviceOverviewService deviceOverviewService, ILogger<DeviceOverviewController> logger)
        {
            _deviceOverviewService = deviceOverviewService;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDeviceOverviewById(int id)
        {
            try
            {
                var deviceOverview = await _deviceOverviewService.GetDeviceOverviewById(id);
                if (deviceOverview == null)
                {
                    _logger.LogWarning($"DeviceOverview with ID {id} not found.");
                    return NotFound($"DeviceOverview with ID {id} not found.");
                }

                return Ok(deviceOverview);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Controller Error: Failed to retrieve device overview with ID {id}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceOverviews()
        {
            try
            {
                var deviceOverviews = await _deviceOverviewService.GetAllDeviceOverviews();
                return Ok(deviceOverviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller Error: Failed to retrieve all device overviews");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDeviceOverview([FromForm] AddDeviceOverviewDTO addDeviceOverviewDto)
        {
            try
            {
                if (addDeviceOverviewDto == null)
                {
                    return BadRequest("Invalid device overview data.");
                }
                // this is dto of the created deviceOverview
                var createdDeviceOverview = await _deviceOverviewService.AddDeviceOverview(addDeviceOverviewDto);

                if (createdDeviceOverview == null)
                {
                    return BadRequest("Failed to create device overview.");
                }

                // Best practice: Return 201 Created with the Location header
                return CreatedAtAction(nameof(GetDeviceOverviewById), new { id = createdDeviceOverview.id }, createdDeviceOverview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller Error: Failed to add device overview");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeviceOverview(int id, [FromBody] UpdateDeviceOverviewDTO updateDeviceOverviewDto)
        {
            try
            {
                if (updateDeviceOverviewDto == null || id <= 0)
                {
                    return BadRequest("Invalid input data.");
                }
                

                var updatedDeviceOverview = await _deviceOverviewService.UpdateDeviceOverview(id, updateDeviceOverviewDto);

                if (updatedDeviceOverview == null)
                {
                    return NotFound($"DeviceOverview with ID {id} not found or not updated.");
                }

                // Return 200 OK with updated data
                return Ok(updatedDeviceOverview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Controller Error: Failed to update device overview with ID {id}");
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet("GetDeviceTypeByID/{id:int}")]
        public async Task<IActionResult> GetDeviceTypeById(int id)
        {
            try
            {
                var deviceType = await _deviceOverviewService.GetDeviceTypeById(id);
                if (deviceType == null)
                {
                    _logger.LogWarning($"Device Type with ID {id} not found.");
                    return NotFound($"Device Type with ID {id} not found.");
                }

                return Ok(deviceType);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Controller Error: Failed to retrieve device type with ID {id}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("UpdateDeviceQuantity")]
        public async Task<IActionResult> UpdateDeviceQuantity(int id, int quantity)
        {
            var updated = await _deviceOverviewService.UpdateDeviceQuantity(id, quantity);

            if (!updated)
            {
                return NotFound($"DeviceOverview with ID {id} not found.");
            }

            return NoContent(); // 204 No Content (successful update, no return body)
        }

        [HttpGet("DecrementAvailableQuantity/{id:int}")]
        public async Task<IActionResult> DecrementAvailableQuantity(int id)
        {
            var updated = await _deviceOverviewService.DecrementAvailableQuantity(id);

            if (!updated)
            {
                return NotFound($"DeviceOverview with ID {id} not found.");
            }

            return NoContent();
        }

    }
}
