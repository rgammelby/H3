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
        Task<DeviceDTO> Get(int id);
        Task<DeviceDTO[]> GetAll();
        Task Add(AddSingleDeviceDTO device);
        Task Update(UpdateDeviceDTO device);
        Task DeactivateDevice(int id);
    }
    public class DeviceRepository: IDeviceRepository
    {
        private readonly Context _context;
        public DeviceRepository(Context db)
        {
            _context = db;
        }

        public async Task<DeviceDTO> Get(int id)
        {
            try
            {
                if (id == 0) return null;

                SingleDevice device = await _context.SingleDevices.Where(db => db.id == id).FirstAsync();

                return new DeviceDTO
                {
                    id = device.id,
                    description = device.description,
                    location_id = device.location,
                    device_overview_id = device.device_overview_id,
                    qr = device.qr,
                    status = device.status,
                    is_archived = device.is_archived,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while getting device: {id}\nError: ", ex.Message);
                return null;
            }
        }

        public async Task<DeviceDTO[]> GetAll()
        {
            try
            {
                DeviceDTO[] device = await _context.SingleDevices.Select(db => new DeviceDTO
                {
                    id = db.id,
                    description = db.description,
                    location_id = db.location,
                    device_overview_id = db.device_overview_id,
                    qr = db.qr,
                    status = db.status,
                    is_archived = db.is_archived,
                }).ToArrayAsync();

                return device;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while getting all devices\nError: ", ex.Message);
                return null;
            }
        }

        public async Task Add(AddSingleDeviceDTO device)
        {
            try
            {
                DeviceOverview oldOverview = await _context.DeviceOverview.Where(db => db.model.Contains(device.model)).FirstOrDefaultAsync();

                int deviceOverview_id = 0;

                if (oldOverview == null)
                {
                    oldOverview = await AddDeviceOverview(device);
                }
                deviceOverview_id = oldOverview.id;

                bool exists = await _context.DeviceOverview.AnyAsync(d => d.id == deviceOverview_id);
                if (!exists)
                {
                    throw new Exception($"DeviceOverview with ID {deviceOverview_id} not found.");
                }

                SingleDevice newDevice = new SingleDevice
                {
                    device_overview_id = deviceOverview_id,
                    description = device.description,
                    location = device.location_id,
                    qr = device.qr,
                    status = device.status_id,
                    is_archived = device.is_archived
                };
                _context.SingleDevices.Add(newDevice);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding device\nError: {ex.Message}");
            }
        }

        // Used to make overview there is no overview with this specified model
        private async Task<DeviceOverview> AddDeviceOverview(AddSingleDeviceDTO device)
        {
            try
            {
                DeviceOverview newOverview = new DeviceOverview
                {
                    device_type = device.device_type,
                    model = device.model,
                    available_qty = device.available_qty,
                    qty = device.qty,
                    last_ordered = device.last_ordered,
                    image = device.image
                };

                _context.DeviceOverview.Add(newOverview);
                var saved = await _context.SaveChangesAsync();

                return newOverview;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task Update(UpdateDeviceDTO device)
        {
            try
            {
                SingleDevice newDevice = await _context.SingleDevices.Where(db => db.id == device.id).FirstOrDefaultAsync();

                if (newDevice == null) throw new Exception("Could not find device to be updated");

                newDevice.description = !string.IsNullOrEmpty(device.description) ? device.description : newDevice.description;
                newDevice.location = device.location_id != 0 ? device.location_id : newDevice.location;
                newDevice.qr = !string.IsNullOrEmpty(device.qr) ? device.qr : newDevice.qr;
                newDevice.status = device.status != 0 ? device.status : newDevice.status;
                newDevice.is_archived = device.is_archived;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating device: {device.id}\nError: ", ex.Message);
            }
        }

        public async Task DeactivateDevice(int id)
        {
            try
            {
                SingleDevice newDevice = await _context.SingleDevices.Where(db => db.id == id).FirstOrDefaultAsync();

                if (newDevice == null) throw new Exception("Could not find the device to be deactivated");

                newDevice.is_archived = true;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deactiveting device: {id}\nError: ", ex.Message);
            }
        }
    }
}
