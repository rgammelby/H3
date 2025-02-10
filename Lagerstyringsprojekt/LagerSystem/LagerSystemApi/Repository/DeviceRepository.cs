global using LagerstyringClassLibrary;
global using LagerstyringClassLibrary.Models;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Repository
{
    
    /// <summary>
    ///  The repository is responsible for interacting with the database. It:
    /// Fetches, adds, updates, or deletes data from the database.
    /// Returns domain models (like SingleDevice or DTOs if necessary).
    /// Does not handle business rules or logic.
    /// </summary>
    public class DeviceRepository: IDeviceRepository
    {
        private readonly Context _context;
        public DeviceRepository(Context db)
        {
            _context = db;
        }

        public async Task<List<SingleDevice>> GetSingleDevicesByModel(string model)
        {
            try
            {
                var deviceOverviewIds = await _context.DeviceOverview
                                                      .Where(d => d.model.Contains(model))
                                                      .Select(d => d.id)
                                                      .ToListAsync();

                return await _context.SingleDevices
                                      .Where(s => deviceOverviewIds.Contains(s.id))
                                      .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while retrieving all devices with model name '{model}'.\nError: {ex.Message}");
            }
        }


        public async Task<SingleDevice?> GetDeviceById(int id)
        {
            try
            {
                return await _context.SingleDevices.SingleOrDefaultAsync(d => d.id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while retriving device with id {id}.\nError: {ex.Message}");
            }
            // If no device found, returns null (handled in service layer)
        }

        public async Task<List<SingleDevice>> GetAllDevices()
        {
            try
            {
                return await _context.SingleDevices.ToListAsync() ?? new List<SingleDevice>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while retrieving all devices.\nError: {ex.Message}");
            }
            // Ensures it never returns null, only an empty list
        }

        public async Task<SingleDevice> AddDevice(SingleDevice device)
        {
            try
            {
                _context.SingleDevices.Add(device);
                // save the change first, EF Core inserts into the database and assigns an ID.
                await _context.SaveChangesAsync();
                //  the complete entity (with generated id) is returned.
                return device;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while adding a new device.\nError: {ex.Message}");
            }
        }

        // Used to make overview if there is no overview with this specified model
        //private async Task<DeviceOverview> AddDeviceOverview(AddSingleDeviceDTO device)
        //{
        //    try
        //    {
        //        DeviceOverview newOverview = new DeviceOverview
        //        {
        //            device_type = device.device_type,
        //            model = device.model,
        //            available_qty = device.available_qty,
        //            qty = device.qty,
        //            last_ordered = device.last_ordered,
        //            image = device.image
        //        };

        //        _context.DeviceOverview.Add(newOverview);
        //        var saved = await _context.SaveChangesAsync();

        //        return newOverview;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


        public async Task<SingleDevice> UpdateDevice(SingleDevice device)
        {
            try
            {
                _context.Entry(device).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                // return the updated domain model
                return device;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while updating device.\nError: {ex.Message}");
            }
        }

        //public async Task DeactivateDevice(int id)
        //{
        //    var device = await GetDeviceById(id);
        //    if (device != null)
        //    {
        //        device.is_archived = true;
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
