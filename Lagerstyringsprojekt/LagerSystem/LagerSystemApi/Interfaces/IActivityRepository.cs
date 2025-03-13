using LagerSystemApi.Models.DTO;
<<<<<<< HEAD
=======
using Microsoft.AspNetCore.Mvc;
>>>>>>> a86a474 (full branches for new activitytype endpoint)

namespace LagerSystemApi.Interfaces
{
    public interface IActivityRepository
    {
        Task<ActivityDTO> Add(ActivityDTO activity);
        Task<UpdateActivityDTO> Update(UpdateActivityDTO activity);
        Task<ActivityDTO> Get(int id);
        Task<ActivityDTO[]> GetAll();
        Task<ActivityDTO[]> GetByLifecycleId(int id);
        Task<ActivityDTO[]> GetByDeviceId(int id);
        /*
        Task<ActivityDTO[]> SearchByType(string key);
        */
<<<<<<< HEAD
=======
        Task<List<ActivityType>> GetAllActivityTypes();
        Task<ActivityDTO[]> GetActivitiesByUserId(int id);
>>>>>>> a86a474 (full branches for new activitytype endpoint)

    }
}
