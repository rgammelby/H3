using LagerSystemApi.Models.DTO;
<<<<<<< HEAD
=======
using Microsoft.AspNetCore.Mvc;
>>>>>>> a86a474 (full branches for new activitytype endpoint)

namespace LagerSystemApi.Interfaces
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
        Task<UpdateActivityDTO> UpdateActivity(UpdateActivityDTO activity);
<<<<<<< HEAD
=======
        Task<List<ActivityTypeDTO>> GetAllActivityTypes();
        Task<ActivityDTO[]> GetActivitiesByUserId(int id);

>>>>>>> a86a474 (full branches for new activitytype endpoint)
    }
}
