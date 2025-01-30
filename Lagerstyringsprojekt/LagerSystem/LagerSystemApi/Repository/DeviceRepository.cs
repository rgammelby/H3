using LagerSystemApi.Data;
using LagerSystemApi.Models.Domain;
using LagerSystemApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Repository
{
    public interface IDeviceRepository
    {
        Task<DeviceDTO> Get(int id);
        Task<DeviceDTO[]> GetAll();
        Task Add(DeviceDTO device);
        Task Update(UpdateDeviceDTO device);
        Task DeactivateDevice(int id);
    }
    public class DeviceRepository: IDeviceRepository
    {
        private readonly LagerSystemDbContext _context;
        public DeviceRepository(LagerSystemDbContext db)
        {
            _context = db;
        }

        public async Task<DeviceDTO> Get(int id)
        {
            try
            {
                if (id == 0) return null;

                SingleDevice device = await _context.Devices.Where(db => db.id == id).FirstAsync();

                return new DeviceDTO
                {
                    id = device.id,
                    description = device.description,
                    location_id = device.location_id,
                    name = device.name,
                    qr = device.qr,
                    status = device.status,
                    is_archived = device.is_archived,
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<DeviceDTO[]> GetAll()
        {
            try
            {
                DeviceDTO[] device = await _context.Devices.Select(db => new DeviceDTO
                {
                    id = db.id,
                    description = db.description,
                    location_id = db.location_id,
                    name = db.name,
                    qr = db.qr,
                    status = db.status,
                    is_archived = db.is_archived,
                }).ToArrayAsync();

                return device;
            }
            catch
            {
                return null;
            }
        }

        public async Task Add(DeviceDTO device)
        {
            try
            {
                SingleDevice newDevice = new SingleDevice
                {
                    description = device.description,
                    location_id = device.location_id,
                    name = device.name,
                    qr = device.qr,
                    status = device.status,
                    is_archived = device.is_archived,
                };
                _context.Devices.Add(newDevice);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
        }

        public async Task Update(UpdateDeviceDTO device)
        {
            try
            {
                SingleDevice newDevice = await _context.Devices.Where(db => db.id == device.id).FirstOrDefaultAsync();

                if (newDevice == null) throw new Exception("Could not find device to be updated");

                newDevice.description = !string.IsNullOrEmpty(device.description) ? device.description : newDevice.description;
                newDevice.location_id = device.location_id != 0 ? device.location_id : newDevice.location_id;
                newDevice.name = !string.IsNullOrEmpty(device.name) ? device.name : newDevice.name;
                newDevice.qr = !string.IsNullOrEmpty(device.qr) ? device.qr : newDevice.qr;
                newDevice.status = device.status != 0 ? device.status : newDevice.status;
                newDevice.is_archived = device.is_archived != default ? device.is_archived : newDevice.is_archived;

                await _context.SaveChangesAsync();
            }
            catch
            {

            }
        }

        public async Task DeactivateDevice(int id)
        {
            try
            {
                SingleDevice newDevice = await _context.Devices.Where(db => db.id == id).FirstOrDefaultAsync();

                if (newDevice == null) throw new Exception("Could not find the device to be deactivated");

                newDevice.is_archived = true;

                await _context.SaveChangesAsync();
            }
            catch
            {

            }
        }
    }
}
