using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Repository
{
    public class StatusTypeRepository : IStatusTypeRepository
    {
        private readonly Context _context;
        public StatusTypeRepository(Context db)
        {
            _context = db;
        }

        public async Task<List<StatusType>> GetAllStatusTypes()
        {
            return await _context.StatusTypes.ToListAsync() ?? new List<StatusType>();
        }

        public async Task<StatusType> GetStatusTypeById(int id)
        {
            return await _context.StatusTypes.SingleOrDefaultAsync(d => d.id == id);
        }
    }
}
