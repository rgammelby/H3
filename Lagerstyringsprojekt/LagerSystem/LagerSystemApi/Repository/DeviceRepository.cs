//using LagerSystemApi.Data;
global using LagerstyringClassLibrary;
global using LagerstyringClassLibrary.Models;
//using LagerSystemApi.Models.Domain;
using LagerSystemApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Repository
{
    public interface IDeviceRepository
    {
        // TODO: GetFilteredDevices  // skal hente devices ud fra et filter; type
        Task<List<SingleDevice>> GetSingleDevicesByModel(string model);
        Task<SingleDevice?> GetDeviceById(int id); // Returns domain model
        Task<List<SingleDevice>> GetAllDevices();  // Returns list of domain models
        Task<SingleDevice> AddDevice(SingleDevice device);       // Accepts domain model for adding
        Task<SingleDevice> UpdateDevice(SingleDevice device);    // Updates a domain model
        // Task DeactivateDevice(int id);                 // Deactivate a device by id
    }
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
            var deviceOverviewIds = await _context.DeviceOverview
                                                  .Where(d => d.model.Contains(model))
                                                  .Select(d => d.id)
                                                  .ToListAsync();

            return await _context.SingleDevices
                                  .Where(s => deviceOverviewIds.Contains(s.id))
                                  .ToListAsync();
        }


        public async Task<SingleDevice?> GetDeviceById(int id)
        {
           return await _context.SingleDevices.SingleOrDefaultAsync( d => d.id == id);
            // If no device found, returns null (handled in service layer)
        }

        public async Task<List<SingleDevice>> GetAllDevices()
        {
           return await _context.SingleDevices.ToListAsync() ?? new List<SingleDevice>();
            // Ensures it never returns null, only an empty list
        }

        public async Task<SingleDevice> AddDevice(SingleDevice device)
        {
            _context.SingleDevices.Add(device);
            // save the change first, EF Core inserts into the database and assigns an ID.
            await _context.SaveChangesAsync();
            //  the complete entity (with generated id) is returned.
            return device;
        }

        // Used to make overview there is no overview with this specified model
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
            _context.Entry(device).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            // return the updated domain model
            return device;
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
