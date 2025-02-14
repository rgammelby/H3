using LagerSystemApi.Models.DTO;

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

    }
}
