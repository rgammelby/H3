using AutoMapper;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Services
{
    public class LogService: ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        public LogService(ILogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository; 
            _mapper = mapper;
        }
        public async Task<List<LogDTO>> GetAllLogs()
        {
            try
            {
                var logs = await _logRepository.GetAllLogs();

                // Handle empty or null lists
                if (logs == null)
                {
                    return new List<LogDTO>(); // Return empty list instead of throwing an exception
                }

                return _mapper.Map<List<LogDTO>>(logs);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving logs from the database.", ex);
            }
        }
    }
}
