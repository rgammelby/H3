using LagerSystemApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace LagerSystemApi.Repository
{
    public class LogRepository: ILogRepository
    {
        private readonly Context _context;
        // private readonly ILogger<Log> _logger;

        public LogRepository(Context db)
        {
            _context = db;
            //_logger = logger;
        }

        public async Task<List<Log>> GetAllLogs()
        {
            try
            {
                return await _context.Logs.ToListAsync() ?? new List<Log>();
            }


            catch (Exception ex)
            {
                throw new Exception("Unexpected error while retrieving logs.", ex);
            }
        }

        //public async Task<Log?> GetLogById(int id)
        //{
        //    try
        //    {
        //        return await _context.Logs.SingleOrDefaultAsync(l => l.id == id);
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        throw new Exception($"Database error while retrieving device overview with ID {id}: {dbEx.Message}", dbEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Unexpected error while retrieving device overview with ID {id}: {ex.Message}", ex);
        //    }
        //}
    }
}
