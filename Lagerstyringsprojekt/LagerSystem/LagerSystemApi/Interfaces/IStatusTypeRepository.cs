using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface IStatusTypeRepository
    {
        Task<List<StatusType>> GetAllStatusTypes();
        Task<StatusType> GetStatusTypeById(int id);
    }
}
