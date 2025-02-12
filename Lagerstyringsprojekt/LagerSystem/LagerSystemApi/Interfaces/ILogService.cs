using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Interfaces
{
    public interface ILogService
    {
        Task<List<LogDTO>> GetAllLogs();
    }
}
