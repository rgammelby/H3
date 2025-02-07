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
        Task Update(UpdateActivityDTO activity);
    }

    // TODO: Add logging in the catch blocks
    
    public class ActivityController: ControllerBase, IActivityController
    {
        private IActivityService _activity;
        public ActivityController(IActivityService service)
        {
            _activity = service;
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
                return BadRequest($"Error while getting Activity with id: {id}");
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
                return BadRequest($"Error while getting activities by deviceId: {id}");
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
                return BadRequest("Error getting all activities");
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
                return BadRequest("Error while adding a new activity");
            }
        }

        [HttpPut("UpdateActivity")]
        public async Task Update(UpdateActivityDTO activity)
        {
            try
            {
                await _activity.UpdateActivity(activity);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
