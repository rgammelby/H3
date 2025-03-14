using LagerSystemApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;

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
        Task<List<ActivityTypeDTO>> GetAllActivityTypes();
        Task<ActivityDTO[]> GetActivitiesByUserId(int id);
        Task<ActivityTypeDTO> GetActivityTypeById(int id);

    }
}
