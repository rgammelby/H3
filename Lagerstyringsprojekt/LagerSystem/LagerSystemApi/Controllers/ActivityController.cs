using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.Models.DTO;
using LagerSystemApi.Services;
using LagerSystemApi.Data;

namespace LagerSystemApi.Controllers
{
    public interface IActivityController
    {
        Task<ActivityDTO> Get(int id);
        Task<ActivityDTO[]> GetAll();
        Task<ActivityDTO[]> GetByDeviceId(int id);
        Task Add(ActivityDTO activity);
        Task Update(UpdateActivityDTO activity);
    }
    public class ActivityController: IActivityController
    {
        private LagerSystemDbContext _context;
        private IActivityService _activity;
        public ActivityController(LagerSystemDbContext db, IActivityService service)
        {
            _context = db;
            _activity = service;
        }
        [HttpGet("GetActivity")]
        public async Task<ActivityDTO> Get(int id)
        {
            return await _activity.Get(id);
        }

        [HttpGet("GetDeviceById")]
        public async Task<ActivityDTO[]> GetByDeviceId(int id)
        {
            return await _activity.GetByDeviceId(id);
        }

        [HttpGet("GetAllActivities")]
        public async Task<ActivityDTO[]> GetAll()
        {
            return await _activity.GetAll();
        }

        [HttpPost("AddActivity")]
        public async Task Add(ActivityDTO activity)
        {
            await _activity.AddActivity(activity);
        }

        [HttpPut("UpdateActivity")]
        public async Task Update(UpdateActivityDTO activity)
        {
            await _activity.UpdateActivity(activity);
        }
    }
}
