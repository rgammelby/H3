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
        void AddActivity(ActivityDTO activity);
        void UpdateActivity(UpdateActivityDTO activity);
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
            if (id == 0) return null;

            return _activity.Get(id);
        }
        public Task<ActivityDTO[]> GetAll()
        {
            return _activity.GetAll();
        }

        public Task<ActivityDTO[]> GetByDeviceId(int id)
        {
            if (id == 0) return null;
            return _activity.GetByDeviceId(id);
        }

        public Task<ActivityDTO[]> GetByLifecycleId(int id)
        {
            if (id == 0) return null;

            return _activity.GetByLifecycleId(id);
        }
        /*
        public Task<ActivityDTO[]> SearchByTypes(string key)
        {
            if (String.IsNullOrEmpty(key)) return null;
            return _activity.SearchByTypes(key);
        }
        */
        public void AddActivity(ActivityDTO activity)
        {
            if (activity != null && !HasNullFields(activity))
            {
                _activity.Add(activity);
            }
        }
        public void UpdateActivity(UpdateActivityDTO activity)
        {
            if (activity != null && !HasNullFields(activity))
            {
                _activity.Update(activity);
            }
        }

        /*
         * These are private methods used for easier use
        */

        private bool HasNullFields(ActivityDTO activity)
        {
            return activity.notes == null ||
                   activity.activity_type == null ||
                   activity.end_date == null ||
                   activity.created_at == null ||
                   activity.device_id == null ||
                   activity.start_date == null ||
                   activity.lifecycle_id == null;
        }

        private bool HasNullFields(UpdateActivityDTO activity)
        {
            return activity.notes == null ||
                   activity.activity_type == null ||
                   activity.end_date == null ||
                   activity.created_at == null ||
                   activity.device_id == null ||
                   activity.start_date == null ||
                   activity.lifecycle_id == null;
        }
    }
}
