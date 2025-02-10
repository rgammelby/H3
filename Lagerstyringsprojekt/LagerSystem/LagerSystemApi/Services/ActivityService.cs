using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;

namespace LagerSystemApi.Services
{
    public interface IActivityService
    {
        Task<ActivityDTO> Get(int id);
        Task<ActivityDTO[]> GetAll();
        Task<ActivityDTO[]> GetByDeviceId(int id);
        Task<ActivityDTO[]> GetByLifecycleId(int id);
        /*
        Task<ActivityDTO[]> SearchByType(string type);
        */
        Task<ActivityDTO> AddActivity(ActivityDTO activity);
        Task UpdateActivity(UpdateActivityDTO activity);
    }
    public class ActivityService : IActivityService
    {
        IActivityRepository _activity;
        public ActivityService(IActivityRepository repo)
        {
            _activity = repo;
        }

        public Task<ActivityDTO> Get(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("Not a valid id");

                return _activity.Get(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Task<ActivityDTO[]> GetAll()
        {
            try
            {
                return _activity.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<ActivityDTO[]> GetByDeviceId(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("Not a valid id");
                return _activity.GetByDeviceId(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<ActivityDTO[]> GetByLifecycleId(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("Not a valid id");

                return _activity.GetByLifecycleId(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /*
        public Task<ActivityDTO[]> SearchByTypes(string key)
        {
            if (String.IsNullOrEmpty(key)) return null;
            return _activity.SearchByTypes(key);
        }
        */
        public async Task<ActivityDTO> AddActivity(ActivityDTO activity)
        {
            try
            {
                if (activity != null && !HasNullFields(activity))
                {
                    return await _activity.Add(activity);
                }
                throw new Exception("Some properties were not valid");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateActivity(UpdateActivityDTO activity)
        {
            try
            {
                if (activity != null && !HasNullFields(activity))
                {
                    await _activity.Update(activity);
                }
                throw new Exception("Some properties were not valid");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*
         * These are private methods used for easier use
        */

        private bool HasNullFields(ActivityDTO activity)
        {
            return string.IsNullOrEmpty(activity.notes) ||
                   activity.activity_type == 0 ||
                   activity.user_id == 0 ||
                   activity.end_date == default(DateTime) ||
                   activity.device_id == 0 ||
                   activity.start_date == default(DateTime) ||
                   activity.lifecycle_id == new Guid();
        }

        private bool HasNullFields(UpdateActivityDTO activity)
        {
            return string.IsNullOrEmpty(activity.notes) ||
                   activity.activity_type == 0 ||
                   activity.end_date == default(DateTime) ||
                   activity.device_id == 0 ||
                   activity.start_date == default(DateTime) ||
                   activity.lifecycle_id != new Guid();
        }
    }
}
