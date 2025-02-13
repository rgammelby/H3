using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface IStatusTypeService
    {
        Task<List<StatusTypeDTO>> GetAllStatusTypes();   // Fetch all devices (DTO)
    }
}
