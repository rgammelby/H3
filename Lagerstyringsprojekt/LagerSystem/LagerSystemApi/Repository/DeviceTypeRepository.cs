using Docker.DotNet.Models;
using LagerSystemApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Repository
{
    public class DeviceTypeRepository : IDeviceTypeRepository
    {
        private readonly Context _context;
        public DeviceTypeRepository(Context context)
        {
            _context = context;
        }
        public async Task<List<DeviceType>> GetAllDeviceTypes()
        {
            return await _context.DeviceTypes.ToListAsync() ?? new List<DeviceType>();
        }
    }
}
