using LagerSystemApi.Models.DTO;
using AutoMapper;
using LagerSystemApi.Interfaces;

namespace LagerSystemApi.Services
{
    /// <summary>
    /// Validates inputs and ensures rules are applied (e.g., checking is_archived).
    /// Converts DTOs to domain models(and vice versa).
    /// Calls the repository to perform the actual database operations.
    /// </summary>
    public class DeviceService: IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<DeviceService> _logger; // Inject Logger
        private readonly IMapper _mapper;
        public DeviceService(IDeviceRepository device, ILogger<DeviceService> logger, IMapper mapper)
        {
            _deviceRepository = device;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<DeviceDTO>> GetSingleDevicesByModel(string model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model)) throw new Exception("GetSingleDevicesByModel failed: Model can not be empty or null");

                List<SingleDevice> singleDevices = await _deviceRepository.GetSingleDevicesByModel(model);

                return _mapper.Map<List<DeviceDTO>>(singleDevices);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Fetch a device and convert to DTO
        public async Task<DeviceDTO?> GetDevice(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("GetDevice failed: Id can not be 0 or less");

                var device = await _deviceRepository.GetDeviceById(id);
                if (device == null) throw new Exception($"GetDeviceById failed: Device with id: {id} was not found");

                //return new DeviceDTO
                //{
                //    id = device.id,
                //    device_overview_id = device.device_overview_id,
                //    is_archived = device.is_archived,
                //    description = device.description,
                //    status = device.status,
                //    location = device.location,
                //    qr = device.qr,
                //};

                var deviceDto = _mapper.Map<DeviceDTO>(device);

                return deviceDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        // Fetch all devices and convert to DTOs
        public async Task<List<DeviceDTO>> GetAllDevices()
        {
            try
            {
                var devices = await _deviceRepository.GetAllDevices();

                if (devices == null) throw new Exception("GetAllDevices faild: No devices were found");
                //return devices.Select(devices => new DeviceDTO
                //{
                //    id = devices.id,
                //    device_overview_id = devices.device_overview_id,
                //    is_archived = devices.is_archived,
                //    description = devices.description,
                //    status = devices.status,
                //    location = devices.location,
                //    qr = devices.qr
                //}).ToList();

                var deviceDtos = _mapper.Map<List<DeviceDTO>>(devices);

                return deviceDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //  Add a new device (DTO -> Entity)
        public async Task<DeviceDTO?> AddDevice(AddSingleDeviceDTO newDeviceDto)
        {
            try
            {
                if (newDeviceDto == null) throw new Exception("AddDevice failed: newDeviceDto is null.");

                // Validate required fields
                if (newDeviceDto.status <= 0) throw new Exception("AddDevice failed: Status must be greater than 0.");

                if (newDeviceDto.device_overview_id <= 0) throw new Exception("AddDevice failed: Device must be linked to a valid overview.");

                // Set default values if null or empty
                newDeviceDto.description ??= "No description provided";
                // TODO: Call qr code, to create with a url to get this device.
                newDeviceDto.qr ??= "";
                newDeviceDto.status = newDeviceDto.status != 0 ? newDeviceDto.status : 1; //  Default status to `1` (Available)


                //var device = new SingleDevice
                //{
                //    status = newDeviceDto.status,
                //    location = newDeviceDto.location,
                //    device_overview_id = newDeviceDto.device_overview_id,
                //    description = newDeviceDto.description,
                //    qr = newDeviceDto.qr,
                //    is_archived = newDeviceDto.is_archived
                //};

                // 1. map AddSingleDeviceDTO to domain model, no id yet
                var device = _mapper.Map<SingleDevice>(newDeviceDto);

                // 2. save the new device to db
                await _deviceRepository.AddDevice(device);

                // 3. Map the saved device (with ID) back to `DeviceDTO`
                var deviceDto = _mapper.Map<DeviceDTO>(device);

                return deviceDto;


                //return new DeviceDTO
                //{
                //    id = device.id, // EF automatically updates this field
                //    device_overview_id = device.device_overview_id,
                //    description = device.description,
                //    status = device.status,
                //    location = device.location,
                //    qr = device.qr,
                //    is_archived = device.is_archived
                //};
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<DeviceDTO?> UpdateDevice(int id, UpdateDeviceDTO updateDeviceDto)
        {
            try
            {
                // Validate input
                if (updateDeviceDto == null) throw new Exception("UpdateDevice failed: deviceDTO is null.");
                if (id <= 0) throw new Exception($"UpdateDevice failed: Invalid device ID {id}.");

                // get domain model by id
                var device = await _deviceRepository.GetDeviceById(id);

                if (device == null) throw new Exception($"UpdateDevice failed: Device with ID {id} not found.");

                //device.description = updateDeviceDto.description ?? device.description;
                //device.location = updateDeviceDto.location != 0 ? updateDeviceDto.location : device.location;
                //device.qr = updateDeviceDto.qr ?? device.qr;
                //device.status = updateDeviceDto.status != 0 ? updateDeviceDto.status : device.status;
                //device.is_archived = updateDeviceDto.is_archived;

                // AutoMapper updates only non-null properties in `device`
                _mapper.Map(updateDeviceDto, device);

                await _deviceRepository.UpdateDevice(device);

                //return new DeviceDTO
                //{
                //    id = device.id,
                //    device_overview_id = device.device_overview_id,
                //    is_archived = device.is_archived,
                //    description = device.description,
                //    status = device.status,
                //    location = device.location,
                //    qr = device.qr
                //};

                // Convert back to DTO
                return _mapper.Map<DeviceDTO>(device);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DeviceDTO?> DeactivateDevice(int id)
        {
            try
            {
                if (id <= 0) throw new Exception($"DeactivateDevice failed: Invalid device ID {id}.");

                // get domain model by id
                var device = await _deviceRepository.GetDeviceById(id);

                if (device == null) throw new Exception($"DeactivateDevice failed: Device with ID {id} not found.");

                if (device.is_archived) throw new Exception($"Device with id: {id} is already deactivated.");

                    //return new DeviceDTO
                    //{
                    //    id = device.id,
                    //    device_overview_id = device.device_overview_id,
                    //    is_archived = device.is_archived,
                    //    description = device.description,
                    //    status = device.status,
                    //    location = device.location,
                    //    qr = device.qr
                    //};

                    // Map the deactivated to dto then return
                    // return _mapper.Map<DeviceDTO>(device);

                // Update only `is_archived`
                device.is_archived = true;
                device.status = 4;

                await _deviceRepository.UpdateDevice(device);

                // Return the updated DeviceDto
                //return new DeviceDTO
                //{
                //    id = device.id,
                //    device_overview_id = device.device_overview_id,
                //    is_archived = device.is_archived,
                //    description = device.description,
                //    status = device.status,
                //    location = device.location,
                //    qr = device.qr
                //};

                return _mapper.Map<DeviceDTO?>(device);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
