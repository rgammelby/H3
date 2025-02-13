using LagerSystemApi.Interfaces;
using LagerSystemApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LagerSystemApi.Controllers
{
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

        [HttpGet("GetCupboardById/{id:int}")]
        public async Task<IActionResult> GetCupboardById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid Cupboard id." });
                }

                var cupboard = await _locationService.GetCupboardById(id);
                if (cupboard == null)
                {
                    return NotFound(new { message = $"Cupboard with id {id} not found." });
                }
                return Ok(cupboard);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation(ex.Message);
                return BadRequest($"Error getting cupboard by id: {id}");
            }
        }

        [HttpGet("GetRoomById/{id:int}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid Room id." });
                }

                var room = await _locationService.GetRoomById(id);
                if (room == null)
                {
                    return NotFound(new { message = $"Room with id {id} not found." });
                }
                return Ok(room);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation(ex.Message);
                return BadRequest($"Error getting room by id: {id}");
            }
        }
    }
}
