using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceOverviewController : ControllerBase
    {
        private readonly IDeviceOverviewService _deviceOverviewService;
        private readonly ILogger<DeviceOverviewController> _logger;
        // private readonly Context db;

        public DeviceOverviewController(IDeviceOverviewService deviceOverviewService, ILogger<DeviceOverviewController> logger)
        {
            _deviceOverviewService = deviceOverviewService;
            _logger = logger;
            // this.db = db;
        }

        // when archived one device, both available_qty and qty of the deviceOverview decrements
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDeviceOverviewById(int id)
        {
            try
            {
                var deviceOverview = await _deviceOverviewService.GetDeviceOverviewById(id);
                if (deviceOverview == null)
                {
                    _logger.LogInformation($"DeviceOverview with ID {id} not found.");
                    return NotFound($"DeviceOverview with ID {id} not found.");
                }

                return Ok(deviceOverview);

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Controller Error: Failed to retrieve device overview with ID {id}.\nError: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceOverviews()
        {
            try
            {

                //var res = await db.DeviceOverview.Select(x => new
                //{
                //    x.model,
                //    x.id,
                //    x.Devices,
                //    x.image
                //}).ToListAsync();

                //return Ok(res);

                var deviceOverviews = await _deviceOverviewService.GetAllDeviceOverviews();
                return Ok(deviceOverviews);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Controller Error: Failed to retrieve all device overviews\nError: {ex.Message}");
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
                _logger.LogInformation($"Controller Error: Failed to add device overview.\nError: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeviceOverview(int id, [FromForm] UpdateDeviceOverviewDTO updateDeviceOverviewDto)
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
                _logger.LogInformation($"Controller Error: Failed to update device overview with ID {id}.\nError: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
