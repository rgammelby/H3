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
        void Add(ActivityDTO activity);
        void Update(UpdateActivityDTO activity);
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

        /// <summary>
        /// This returns all the activites with this device id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        public void Add(ActivityDTO activity)
        {
            _activity.AddActivity(activity);
        }

        [HttpPut("UpdateActivity")]
        public void Update(UpdateActivityDTO activity)
        {
            _activity.UpdateActivity(activity);
        }
    }
}
