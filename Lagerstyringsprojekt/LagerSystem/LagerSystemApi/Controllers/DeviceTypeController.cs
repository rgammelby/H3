using LagerSystemApi.Interfaces;
using LagerSystemApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LagerSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTypeController : ControllerBase
    {
        private readonly IDeviceTypeService _deviceTypeService;

        public DeviceTypeController(IDeviceTypeService deviceTypeService)
        {
            _deviceTypeService = deviceTypeService;
        }

        [HttpGet("GetAllDeviceTypes")]
        public async Task<IActionResult> GetAllDeviceTypes()
        {
            try
            {
                var deviceTypes = await _deviceTypeService.GetAllDeviceTypes();
                if (deviceTypes == null)
                {
                    return BadRequest(new { message = "No device types found." });
                }
                return Ok(deviceTypes);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation(ex.Message);
                return BadRequest($"Error getting all device types. ");
            }
        }
    }
}
