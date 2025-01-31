using LagerSystemApi.Models.DTO;
using LagerSystemApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LagerSystemApi.Controllers
{
    public interface ILogController
    {
        Task<LogDTO> Get(int id);
        Task<LogDTO[]> GetAll();
        Task<LogDTO[]> Search(string key);
    }
    public class LogController: ILogController
    {
        private Context _context;
        private ILogService _log;
        public LogController(Context context, ILogService log)
        {
            _context = context;
            log = log;
        }

        [HttpGet("GetLog")]
        public async Task<LogDTO> Get(int id)
        {
            return await _log.Get(id);
        }

        [HttpGet("GetAllLogs")]
        public async Task<LogDTO[]> GetAll()
        {
            return await _log.GetAll();
        }

        [HttpGet("SearchLogs")]
        public async Task<LogDTO[]> Search(string key)
        {
            return await _log.Search(key);
        }
    }
}
