using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.Models.DTO;
using LagerSystemApi.Services;

namespace LagerSystemApi.Controllers
{
    public interface IActivityController
    {
        Task<IActionResult> Get(int id);
        Task<IActionResult> GetAll();
        Task<IActionResult> GetByDeviceId(int id);
        Task<IActionResult> Add(ActivityDTO activity);
        Task Update(int id, UpdateActivityDTO activity);
    }
    public class ActivityController : ControllerBase, IActivityController
    {
        private IActivityService _activity;
        private readonly ILogger<ActivityController> _logger;
        public ActivityController(IActivityService service, ILogger<ActivityController> logger)
        {
            _activity = service;
            _logger = logger;
        }
        [HttpGet("GetActivity")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _activity.Get(id));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error getting activity with id: {id}");
            }
        }

        [HttpGet("GetDeviceById")]
        public async Task<IActionResult> GetByDeviceId(int id)
        {
            try
            {
                return Ok(await _activity.GetByDeviceId(id));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error getting activities by device id: {id}");
            }
        }

        [HttpGet("GetAllActivities")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _activity.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error getting all activities");
            }
        }

        [HttpPost("AddActivity")]
        public async Task<IActionResult> Add(ActivityDTO activity)
        {
            try
            {
                return Ok(await _activity.AddActivity(activity));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error adding new activity");
            }
        }

        [HttpPut("UpdateActivity")]
        public async Task Update(int id, [FromBody]UpdateActivityDTO activity)
        {
            try
            {
                await _activity.UpdateActivity(activity);
                Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                BadRequest($"Error updating activity with id: {id}");
            }
        }
    }
}
