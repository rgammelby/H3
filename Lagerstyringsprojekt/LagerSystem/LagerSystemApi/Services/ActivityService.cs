using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;
using AutoMapper;
using LagerSystemApi.Interfaces;
using System.Threading.Tasks;

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
        // Task<ActivityDTO> AddActivity(ActivityDTO activity);
        Task UpdateActivity(UpdateActivityDTO activity);
        Task<List<ActivityTypeDTO>> GetAllActivityTypes();

        Task<ActivityDTO?> BorrowDeviceAsync(AddActivityDTO dto);
        Task<ActivityDTO?> ReturnDeviceAsync(AddActivityDTO dto);
    }
    public class ActivityService : IActivityService
    {
        IActivityRepository _activity;
        private readonly IMapper _mapper;

        // inject device and deviceoverview to update device status and available_qty in deviceOverview
        private readonly IDeviceRepository _deviceRepo;
        private readonly IDeviceOverviewRepository _deviceOverviewRepo;

        public ActivityService(IActivityRepository repo, IMapper mapper, IDeviceOverviewRepository deviceOverviewRepo, IDeviceRepository deviceRepo)
        {
            _activity = repo;
            _mapper = mapper;
            _deviceOverviewRepo = deviceOverviewRepo;
            _deviceRepo = deviceRepo;
        }

        // Borrow activity:
        public async Task<ActivityDTO?> BorrowDeviceAsync(AddActivityDTO addActivityDTO)
        {
            // 1. check device availability
            var device = await _deviceRepo.GetDeviceById(addActivityDTO.device_id);
            var deviceOverview = await _deviceOverviewRepo.GetDeviceOverviewById(device.device_overview_id);
            
            if(device == null || device.status !=1 || device.is_archived == true)
            {
                throw new Exception("Device not found or not available.");
            }

            // 2. generate new lifeCycleId to borrow
            addActivityDTO.lifecycle_id = Guid.NewGuid();

            // 3. create a domain model
            var activityEntity = new Activity
            {
                device_id = addActivityDTO.device_id,
                activity_type = addActivityDTO.activity_type,
                user_id = addActivityDTO.user_id,
                start_date = addActivityDTO.start_date,
                end_date = addActivityDTO.end_date,
                created_on = DateTime.Now,
                notes = addActivityDTO.notes,
                lifecycle_id = addActivityDTO.lifecycle_id,
            };

            // 4. Save via repository (which returns the saved domain entity)
            var savedActivity = await _activity.Add(activityEntity);
           
            // 5. Update device status
            device.status = 3; // Borrowed
            deviceOverview.available_qty --; 

            // 6. update and save in db
            await _deviceOverviewRepo.UpdateDeviceOverview(deviceOverview);
            await _deviceRepo.UpdateDevice(device);

            // 7. return dto
            return _mapper.Map<ActivityDTO>(savedActivity);

        }
        // Return activity:
        public async Task<ActivityDTO?> ReturnDeviceAsync(AddActivityDTO addActivityDTO)
        {
            // 1. check device status == 3/2
            var device = await _deviceRepo.GetDeviceById(addActivityDTO.device_id);
            var deviceOverview = await _deviceOverviewRepo.GetDeviceOverviewById(device.device_overview_id);

            if(deviceOverview == null || device == null || device.status == 1 || device.status == 4)
            {
                throw new Exception("Device not found or cannot be returned.");
            }

            // 2. find the corresponding borrow activity:
            // 2.1 get list of activities with the same device_id 
            var allActivity = await _activity.GetByDeviceId(device.id);

            // 2.2  Filter to only borrow activities (activity_type == 1),
            //      then sort by the creation date descending
            var latestBorrowActivity = allActivity
                .Where(a => a.activity_type == 1)
                .OrderByDescending(a => a.created_at)
                .FirstOrDefault();

            // 3. Now latestBorrowActivity is the most recent borrow for that device
            if (latestBorrowActivity == null)
            {
                throw new Exception("No borrow record found for this device.");
            }

            // 4. create a domain model for the borrowed device
            var activityEntity = new Activity
            {
                device_id = latestBorrowActivity.device_id,
                activity_type = 2,
                user_id = latestBorrowActivity.user_id,
                start_date = latestBorrowActivity.start_date,
                end_date = DateTime.Now,
                created_on = DateTime.Now,
                notes = addActivityDTO.notes,
                lifecycle_id = latestBorrowActivity.lifecycle_id,
            };

            // 5. save the return activity
            var savedReturnActivity = await _activity.Add(activityEntity);

            // 6. update device status and available_qty
            device.status = 1;
            deviceOverview.available_qty++;

            // 7. save the changes
            await _deviceRepo.UpdateDevice(device);
            await _deviceOverviewRepo.UpdateDeviceOverview(deviceOverview);

            // 8. return the newly created return activivty
            return _mapper.Map<ActivityDTO>(savedReturnActivity);
        }

        public async Task<List<ActivityTypeDTO>> GetAllActivityTypes()
        {
            var activityTypes = await _activity.GetAllActivityTypes();

            var activityTypeDTOs = _mapper.Map<List<ActivityTypeDTO>>(activityTypes);

            return activityTypeDTOs;
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

        public async Task<ActivityDTO[]> GetByLifecycleId(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("Not a valid lifecycle id");

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
        //public async Task<ActivityDTO> AddActivity(ActivityDTO activity)
        //{
        //    try
        //    {
        //        if (activity == null || HasNullFields(activity)) throw new Exception("Some properties were not valid");

        //        return await _activity.Add(activity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        public async Task UpdateActivity(UpdateActivityDTO activity)
        {
            try
            {
                if (activity == null || HasNullFields(activity)) throw new Exception("Some properties were not valid");

                await _activity.Update(activity);                
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
