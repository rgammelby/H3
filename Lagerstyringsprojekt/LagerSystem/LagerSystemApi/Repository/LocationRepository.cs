using LagerSystemApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly Context _context;
        public LocationRepository(Context db)
        {
            _context = db;
        }

        public async Task<List<LocationCupboard>> GetAllCupboards()
        {
            return await _context.Cupboards.ToListAsync() ?? new List<LocationCupboard>();
        }

        public async Task<LocationCupboard> GetCupboardById(int id)
        {
            return await _context.Cupboards.SingleOrDefaultAsync(d => d.id == id);
        }


        public async Task<List<LocationRoom>> GetAllRooms()
        {
            return await _context.Rooms.ToListAsync() ?? new List<LocationRoom>();
        }
        public async Task<LocationRoom> GetRoomById(int id)
        {
            return await _context.Rooms.SingleOrDefaultAsync(d => d.id == id);
        }
    }
}
