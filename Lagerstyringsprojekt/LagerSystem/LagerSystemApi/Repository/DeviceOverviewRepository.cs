global using LagerstyringClassLibrary;
global using LagerstyringClassLibrary.Models;
using LagerSystemApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Repository
{
    public class DeviceOverviewRepository : IDeviceOverviewRepository
    {
        private readonly Context _context;
        private readonly ILogger _logger;
        public DeviceOverviewRepository(Context db, ILogger<DeviceOverviewRepository> logger)
        {
            _context = db;
            _logger = logger;
        }
        public async Task<bool> DecrementAvailableQuantity(int id)
        {
            var device = await _context.DeviceOverview.FindAsync(id);

            if (device == null)
            {
                return false;
            }

            device.available_qty--;
            device.qty--;
            

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<DeviceOverview?> GetDeviceOverviewById(int id)
        {
            try
            {
                return await _context.DeviceOverview.SingleOrDefaultAsync(d => d.id == id);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error while retrieving device overview with ID {id}\nDB-Error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while retrieving device overview with ID {id}\nError: {ex.Message}");
            }
        }
        public async Task<List<DeviceOverview>> GetAllDeviceOverviews()
        {
            try
            {
                return await _context.DeviceOverview.ToListAsync() ?? new List<DeviceOverview>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all device overviews\nError{ex.Message}");
            }
        }
        public async Task<DeviceOverview> AddDeviceOverview(DeviceOverview deviceOverview)
        {
            try
            {
                _context.DeviceOverview.Add(deviceOverview);
                // save the change first, EF Core inserts into the database and assigns an ID.
                await _context.SaveChangesAsync();
                //  the complete entity (with generated id) is returned.
                return deviceOverview;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error while adding a new device overview\nDB-Error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while adding a new device overview\nError: {ex.Message}");
            }
        }

        public async Task<DeviceOverview> UpdateDeviceOverview(DeviceOverview deviceOverview)
        {
            try
            {
                _context.Entry(deviceOverview).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                // return the updated domain model
                return deviceOverview;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error while updating device overview with ID {deviceOverview.id}\nDB-Error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while updating device overview with ID {deviceOverview.id}\nError: {ex.Message}");
            }
        }

        public async Task<DeviceOverview?> GetDeviceOverviewByModelAndType(string model, int deviceType)
        {
            try
            {
                return await _context.DeviceOverview
                    .FirstOrDefaultAsync(d => d.model == model && d.device_type == deviceType);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving device overview for Model '{model}' and DeviceType {deviceType}\nError: {ex.Message}");
            }
        }
    }
}
