using LagerSystemApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LagerSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        public interface ILocationController
        {
            Task<IActionResult> GetAllCupboards();
        }
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        [HttpGet("GetAllCupboards")]
        public async Task<IActionResult> GetAllCupboards()
        {
            try
            {
                var cupboards = await _locationService.GetAllCupboards();
                if (cupboards == null)
                {
                    return BadRequest(new { message = "No status types found." });
                }
                return Ok(cupboards);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation(ex.Message);
                return BadRequest($"Error getting all status types. ");
            }
        }
        [HttpGet("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            try
            {
                var rooms = await _locationService.GetAllRooms();
                if (rooms == null)
                {
                    return BadRequest(new { message = "No status types found." });
                }
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation(ex.Message);
                return BadRequest($"Error getting all status types. ");
            }
        }

    }
}
