using LagerSystemApi.Models.DTO;

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
    }
}
