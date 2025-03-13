using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;
using LagerSystemApi.Interfaces;
<<<<<<< HEAD
=======
using AutoMapper;
>>>>>>> a86a474 (full branches for new activitytype endpoint)

namespace LagerSystemApi.Services
{
    public class ActivityService : IActivityService
    {
        IActivityRepository _activity;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository repo, IMapper mapper)
        {
            _activity = repo;
            _mapper = mapper;
        }

        public async Task<ActivityDTO> Get(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("Get failed: Not a valid id");

                ActivityDTO activity = await _activity.Get(id);

                if (activity == null) throw new Exception($"No device foudn with id: {id}");

                return activity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ActivityDTO[]> GetAll()
        {
            try
            {
                ActivityDTO[] activity = await _activity.GetAll();

                if (activity == null) throw new Exception("GetAll failed: no activities found");

                return activity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActivityDTO[]> GetByDeviceId(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("GetByDeviceId failed: Not a valid id");
                ActivityDTO[] activity = await _activity.GetByDeviceId(id);

                if (activity == null) throw new Exception($"No activities found with device id: {id}");

                return activity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

<<<<<<< HEAD
        public async Task<ActivityDTO[]> GetByLifecycleId(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("Not a valid lifecycle id");

=======
        public async Task<ActivityDTO[]> GetActivitiesByUserId(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("GetActivitiesByUserId failed: Not a valid id");
                ActivityDTO[] activity = await _activity.GetActivitiesByUserId(id);

                if (activity == null) throw new Exception($"No activities found with user id: {id}");

                return activity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActivityDTO[]> GetByLifecycleId(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("Not a valid lifecycle id");

>>>>>>> a86a474 (full branches for new activitytype endpoint)
                ActivityDTO[] activity = await _activity.GetByLifecycleId(id);

                if (activity == null) throw new Exception($"No activities fund with lifecycle id: {id}");

                return activity;
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
        public async Task<ActivityDTO> AddActivity(ActivityDTO activity)  // TODO: Implement ContinueActivity()
        {
            try
            {
                if (activity == null || HasNullFields(activity)) throw new Exception("Some properties were not valid");

<<<<<<< HEAD
=======
                activity.lifecycle_id = Guid.NewGuid();
>>>>>>> a86a474 (full branches for new activitytype endpoint)
                return await _activity.Add(activity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UpdateActivityDTO> UpdateActivity(UpdateActivityDTO activity)
        {
            try
            {
                if (activity == null || HasNullFields(activity)) throw new Exception("Some properties were not valid");

                return await _activity.Update(activity);
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
                   activity.start_date == default(DateTime);
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

        public async Task<List<ActivityTypeDTO>> GetAllActivityTypes()
        {
            var activityTypes = await _activity.GetAllActivityTypes();

            var activityTypeDTOs = _mapper.Map<List<ActivityTypeDTO>>(activityTypes);

            return activityTypeDTOs;
        }
    }
}
