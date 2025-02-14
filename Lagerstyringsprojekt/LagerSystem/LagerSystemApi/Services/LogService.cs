using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Services
{
    public interface ILogService
    {
        Task<LogDTO> Get(int id);
        Task<LogDTO[]> GetAll();
        Task<LogDTO[]> Search(string key);
    }
    public class LogService: ILogService
    {
        public Task<LogDTO> Get(int id)
        {
            return null;
        }

        public Task<LogDTO[]> GetAll()
        {
            return null;
        }

        public Task<LogDTO[]> Search(string key)
        {
            return null;
        }
    }
}
