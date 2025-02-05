using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace LagerSystemApi.Services
{
    public interface IDeviceService
    {
        Task<List<DeviceDTO>> GetAllDevices();   // Fetch all devices (DTO)
        Task<DeviceDTO?> GetDevice(int id);    // Fetch a single device (DTO)
        Task<DeviceDTO?> AddDevice(AddSingleDeviceDTO device);
        Task<DeviceDTO?> UpdateDevice(UpdateDeviceDTO update);
        Task<DeviceDTO?> DeactivateDevice(int id);
    }
    /// <summary>
    /// Validates inputs and ensures rules are applied (e.g., checking is_archived).
    /// Converts DTOs to domain models(and vice versa).
    /// Calls the repository to perform the actual database operations.
    /// </summary>
    public class DeviceService: IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<DeviceService> _logger; // Inject Logger
        public DeviceService(IDeviceRepository device, ILogger<DeviceService> logger)
        {
            _deviceRepository = device;
            _logger = logger;
        }
        // Fetch a device and convert to DTO
        public async Task<DeviceDTO?> GetDevice(int id)
        {
            if (id <= 0) return null;

            var device = await _deviceRepository.GetDeviceById(id);
            if (device == null) return null;

            return new DeviceDTO
            {
                id = device.id,
                device_overview_id = device.device_overview_id,
                is_archived = device.is_archived,
                description = device.description,
                status = device.status,
                location = device.location,
                qr = device.qr,
            };
        }
        
        // Fetch all devices and convert to DTOs
        public async Task<List<DeviceDTO>> GetAllDevices()
        {
            var devices = await _deviceRepository.GetAllDevices();
            
            return devices.Select(devices => new DeviceDTO
            {
                id = devices.id,
                device_overview_id = devices.device_overview_id,
                is_archived = devices.is_archived,
                description = devices.description,
                status = devices.status,
                location = devices.location,
                qr = devices.qr
            }).ToList();
        }

        //  Add a new device (DTO -> Entity)
        public async Task<DeviceDTO?> AddDevice(AddSingleDeviceDTO newDeviceDto)
        {
            if (newDeviceDto == null) 
            {
                _logger.LogError("AddDevice failed: newDeviceDto is null.");
                return null;
            }

            // 🔹 Validate required fields
            if (newDeviceDto.status <= 0)
            {
                _logger.LogError("AddDevice failed: Status must be greater than 0.");
                return null;
            }

            if (newDeviceDto.device_overview_id <= 0)
            {
                _logger.LogError("AddDevice failed: Device must be linked to a valid overview.");
                return null;
            }

            // 🔹 Set default values if null or empty
            newDeviceDto.description ??= "No description provided";
            newDeviceDto.qr ??= "";
            newDeviceDto.status = newDeviceDto.status != 0 ? newDeviceDto.status : 1; //  Default status to `1` (Available)
            

            var device = new SingleDevice
            {
                status = newDeviceDto.status,
                location = newDeviceDto.location,
                device_overview_id = newDeviceDto.device_overview_id,
                description = newDeviceDto.description,
                qr = newDeviceDto.qr,
                is_archived = newDeviceDto.is_archived
            };

            await _deviceRepository.AddDevice(device);

            // Return the newly created DeviceDto
            return new DeviceDTO
            {
                id = device.id, // EF automatically updates this field
                device_overview_id = device.device_overview_id,
                description = device.description,
                status = device.status,
                location = device.location,
                qr = device.qr,
                is_archived = device.is_archived
            };
        }
        public async Task<DeviceDTO?> UpdateDevice(UpdateDeviceDTO updateDeviceDto)
        {
            // Validate input
            if(updateDeviceDto == null)
            {
                _logger.LogError("UpdateDevice failed: deviceDTO is null.");
                return null;
            }
            if (updateDeviceDto.id <= 0)
            {
                _logger.LogError($"UpdateDevice failed: Invalid device ID {updateDeviceDto.id}.");
                return null;
            }

            var device = await _deviceRepository.GetDeviceById(updateDeviceDto.id);
            if (device == null) 
            {
                _logger.LogError($"UpdateDevice failed: Device with ID {updateDeviceDto.id} not found.");
                return null;
            }
            device.description = updateDeviceDto.description ?? device.description;
            device.location = updateDeviceDto.location != 0 ? updateDeviceDto.location : device.location;
            device.qr = updateDeviceDto.qr ?? device.qr;
            device.status = updateDeviceDto.status != 0 ? updateDeviceDto.status : device.status;
            device.is_archived = updateDeviceDto.is_archived;

            await _deviceRepository.UpdateDevice(device);

            return new DeviceDTO
            {
                id = device.id,
                device_overview_id = device.device_overview_id,
                is_archived = device.is_archived,
                description = device.description,
                status = device.status,
                location = device.location,
                qr = device.qr
            };
        }

        public async Task<DeviceDTO?> DeactivateDevice(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"DeactivateDevice failed: Invalid device ID {id}.");
                return null;
            }

            var device = await _deviceRepository.GetDeviceById(id);
            if (device == null)
            {
                _logger.LogError($"DeactivateDevice failed: Device with ID {id} not found.");
                return null;
            }

            if (device.is_archived)
            {
                _logger.LogInformation($"DeactivateDevice skipped: Device with ID {id} is already deactivated.");
                return new DeviceDTO
                {
                    id = device.id,
                    device_overview_id = device.device_overview_id,
                    is_archived = device.is_archived,
                    description = device.description,
                    status = device.status,
                    location = device.location,
                    qr = device.qr
                };
            }
            // Update only `is_archived`
            device.is_archived = true;

            await _deviceRepository.UpdateDevice(device);

            _logger.LogInformation($"Device with ID {id} successfully deactivated.");

            // Return the updated DeviceDto

            return new DeviceDTO
            {
                id = device.id,
                device_overview_id = device.device_overview_id,
                is_archived = device.is_archived,
                description = device.description,
                status = device.status,
                location = device.location,
                qr = device.qr
            };
        }


        /*
         * These are private methods only used for validating device info
        */

        //private bool HasNullProps(AddSingleDeviceDTO device)
        //{
        //    return device.available_qty == 0 ||
        //        device.qty == 0 ||
        //        string.IsNullOrEmpty(device.image) ||
        //        device.status_id == 0 ||
        //        string.IsNullOrEmpty(device.description) ||
        //        string.IsNullOrEmpty(device.qr) ||
        //        device.location_id == 0;
        //}

        //private bool HasNullProps(UpdateDeviceDTO device)
        //{
        //    return device.id == 0 ||
        //        device.status == 0 ||
        //        string.IsNullOrEmpty(device.description) ||
        //        string.IsNullOrEmpty(device.qr) ||
        //        device.location_id == 0;
        //}
    }
}
