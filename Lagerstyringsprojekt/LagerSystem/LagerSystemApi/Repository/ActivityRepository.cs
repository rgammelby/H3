using LagerSystemApi.Models.DTO;
using LagerstyringClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Repository
{
    public interface IActivityRepository
    {
        Task Add(ActivityDTO activity);
        Task Update (UpdateActivityDTO activity);
        Task<ActivityDTO> Get(int id);
        Task<ActivityDTO[]> GetAll();
        Task<ActivityDTO[]> GetByLifecycleId(int id);
        Task<ActivityDTO[]> GetByDeviceId(int id);
        /*
        Task<ActivityDTO[]> SearchByType(string key);
        */

    }
    public class ActivityRepository: IActivityRepository
    {
        private readonly Context _context;
        public ActivityRepository(Context db)
        {
            _context = db;
        }
        public async Task Add(ActivityDTO activity)
        {
            try
            {
                Activity newActivity = new Activity
                {
                    device_id = activity.device_id,
                    user_id = activity.user_id,
                    activity_type = activity.activity_type,
                    created_on = DateTime.Now,
                    start_date = activity.start_date,
                    end_date = activity.end_date,
                    lifecycle_id = activity.lifecycle_id,
                    notes = activity.notes,
                };

                _context.Activities.Add(newActivity);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Add activity was not succesful");
            }
        }

        public async Task Update(UpdateActivityDTO activity)
        {
            try
            {
                Activity newActivity = await _context.Activities.Where(db => db.id == activity.id).FirstAsync();

                if (newActivity == null) throw new Exception("Could not find the activity to be updated");

                newActivity.device_id = activity.device_id != 0 ? activity.device_id : newActivity.device_id;
                newActivity.start_date = activity.start_date != default ? activity.start_date : newActivity.start_date;
                newActivity.created_on = activity.created_at != default ? activity.created_at : newActivity.created_on;
                newActivity.end_date = activity.end_date != default ? activity.end_date : newActivity.end_date;
                newActivity.notes = !string.IsNullOrEmpty(activity.notes) ? activity.notes : newActivity.notes;
                newActivity.activity_type = activity.activity_type != 0 ? activity.activity_type : newActivity.activity_type;
                newActivity.lifecycle_id = activity.lifecycle_id;


                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Update activity was not succesful");
            }
        }

        public async Task<ActivityDTO> Get(int id)
        {
            try
            {
                ActivityDTO activity = await _context.Activities.Where(db => db.id == id).Select(db => new ActivityDTO
                {
                    id = db.id,
                    notes = db.notes,
                    activity_type = db.activity_type,
                    created_at = db.created_on,
                    start_date = db.start_date,
                    end_date = db.end_date,
                    lifecycle_id = db.lifecycle_id,
                    device_id = db.device_id,
                }).FirstAsync();

                return activity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ActivityDTO[]> GetAll()
        {
            try
            {
                ActivityDTO[] activites = await _context.Activities.Select(db => new ActivityDTO
                {
                    id = db.id,
                    notes = db.notes,
                    activity_type = db.activity_type,
                    created_at = db.created_on,
                    start_date = db.start_date,
                    end_date = db.end_date,
                    lifecycle_id = db.lifecycle_id,
                    device_id = db.device_id,
                }).ToArrayAsync();

                return activites;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ActivityDTO[]> GetByLifecycleId(int id)
        {
            try
            {
                ActivityDTO[] activities = await _context.Activities.Where(db => db.id == id).Select(db => new ActivityDTO
                {
                    id = db.id,
                    notes = db.notes,
                    activity_type = db.activity_type,
                    created_at = db.created_on,
                    start_date = db.start_date,
                    end_date = db.end_date,
                    lifecycle_id = db.lifecycle_id,
                    device_id = db.device_id,
                }).ToArrayAsync();

                return activities;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ActivityDTO[]> GetByDeviceId(int id)
        {
            try
            {
                ActivityDTO[] activities = await _context.Activities.Where(db => db.device_id == id).Select(db => new ActivityDTO
                {
                    id = id,
                    notes = db.notes,
                    activity_type = db.activity_type,
                    created_at = db.created_on,
                    start_date = db.start_date,
                    end_date = db.end_date,
                    lifecycle_id = db.lifecycle_id,
                    device_id = db.device_id,

                }).ToArrayAsync();
                return activities;
            }
            catch
            {
                return null;
            }
        }

        /*
         * Will be used when we have made EF Core
        public async Task<ActivityDTO[]> SearchByType(string key)
        {
            try
            {
                
            }
            catch
            {

            }
        }
        */
    }
}
