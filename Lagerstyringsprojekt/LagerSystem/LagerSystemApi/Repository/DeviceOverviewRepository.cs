global using LagerstyringClassLibrary;
global using LagerstyringClassLibrary.Models;
using LagerSystemApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using LagerSystemApi.Models.DTO;

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

        public async Task<DeviceOverview?> GetDeviceOverviewById(int id)
        {
            try
            {
                return await _context.DeviceOverview.SingleOrDefaultAsync(d => d.id == id);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Database error while retrieving device overview with ID {id}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while retrieving device overview with ID {id}");
                throw;
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
                _logger.LogError(ex, "Error retrieving all device overviews");
                throw;
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
                _logger.LogError(dbEx, "Database error while adding a new device overview");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding a new device overview");
                throw;
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
                _logger.LogError(dbEx, $"Database error while updating device overview with ID {deviceOverview.id}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while updating device overview with ID {deviceOverview.id}");
                throw;
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
                _logger.LogError(ex, $"Error retrieving device overview for Model '{model}' and DeviceType {deviceType}");
                return null;
            }
        }

        public async Task<DeviceType> GetDeviceTypeById(int id)
        {
            try
            {
                return await _context.DeviceTypes.SingleOrDefaultAsync(d => d.id == id);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Database error while retrieving device type with ID {id}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while retrieving device type with ID {id}");
                throw;
            }
        }

        public async Task<bool> UpdateDeviceQuantity(int id, int quantity)
        {
            var device = await _context.DeviceOverview.FindAsync(id);

            if (device == null)
            {
                return false; // Device not found
            }

            device.qty += quantity;
            device.available_qty += quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DecrementAvailableQuantity(int id)
        {
            var device = await _context.DeviceOverview.FindAsync(id);

            if (device == null)
            {
                return false;
            }

            device.available_qty--;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
