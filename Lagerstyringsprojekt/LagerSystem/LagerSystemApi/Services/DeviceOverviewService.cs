using AutoMapper;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Services
{
    public class DeviceOverviewService : IDeviceOverviewService
    {

        private readonly IDeviceOverviewRepository _deviceOverviewRepository;
        private readonly IUploadImages _uploadImages;
        private readonly ILogger<DeviceOverviewService> _logger;
        private readonly IMapper _mapper;

        public DeviceOverviewService(IDeviceOverviewRepository deviceOverviewRepository, IUploadImages uploadImages, ILogger<DeviceOverviewService> logger, IMapper mapper)
        {
            _deviceOverviewRepository = deviceOverviewRepository;
            _uploadImages = uploadImages;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<DeviceOverviewDTO?> GetDeviceOverviewById(int id)
        {
            try
            {
                if (id <= 0) throw new Exception($"GetDeviceOverviewById failed: Invalid ID {id}.");

                var deviceOverview = await _deviceOverviewRepository.GetDeviceOverviewById(id);

                if (deviceOverview == null) throw new Exception($"DeviceOverview with id {id} was not found.");

                return _mapper.Map<DeviceOverviewDTO>(deviceOverview);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<DeviceOverviewDTO>> GetAllDeviceOverviews()
        {
            try
            {
                var deviceOverviews = await _deviceOverviewRepository.GetAllDeviceOverviews();

                if (deviceOverviews == null) throw new Exception("GetAllDeviceOverviews failed: no devices found");

                return _mapper.Map<List<DeviceOverviewDTO>>(deviceOverviews);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<DeviceOverviewDTO?> AddDeviceOverview(AddDeviceOverviewDTO addDeviceOverviewDto)
        {
            try
            {
                // Validate input fields (e.g., device_type > 1, model is not empty).
                if (addDeviceOverviewDto == null) throw new Exception("AddDeviceOverview failed: Input DTO is null.");
                if (string.IsNullOrWhiteSpace(addDeviceOverviewDto.model)) throw new Exception("AddDeviceOverview failed: Model cannot be empty.");
                if (addDeviceOverviewDto.device_type < 1) throw new Exception($"AddDeviceOverview failed: DeviceType {addDeviceOverviewDto.device_type} is invalid. Must be > 0.");
                if (addDeviceOverviewDto.qty < 0 || addDeviceOverviewDto.available_qty < 0) throw new Exception($"AddDeviceOverview failed: Qty {addDeviceOverviewDto.qty} and AvailableQty {addDeviceOverviewDto.available_qty} must be >= 0.");

                try
                {
                    // 1. Check if a device with the same `model` and `device_type` already exists
                    var existingDeviceOverview = await _deviceOverviewRepository.GetDeviceOverviewByModelAndType(addDeviceOverviewDto.model, addDeviceOverviewDto.device_type);

                    if (existingDeviceOverview != null)
                    {
                        throw new Exception($"DeviveOverview with Model {addDeviceOverviewDto?.model} and DeviceType {addDeviceOverviewDto?.device_type} already exists.");

                        //// Map new data from DTO while keeping the same ID
                        //_mapper.Map(addDeviceOverviewDto, existingDeviceOverview);
                        //existingDeviceOverview.last_ordered = DateTime.UtcNow; // Update last ordered date

                        //var updatedDeviceOverview = await _deviceOverviewRepository.UpdateDeviceOverview(existingDeviceOverview);

                        //// return dto of updated overview
                        //return _mapper.Map<DeviceOverviewDTO>(updatedDeviceOverview);
                    }

                    // ---------------TODO : image -------------------------------------
                    // if user upload a picture, call gateway for pic-handling then save it to addDeviceOverviewDto

                    string filePath = await _uploadImages.SaveImage(addDeviceOverviewDto.image);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        addDeviceOverviewDto.image_path = filePath;
                    }

                    var deviceOverview = _mapper.Map<DeviceOverview>(addDeviceOverviewDto);

                    //  save deviceOverview to db
                    await _deviceOverviewRepository.AddDeviceOverview(deviceOverview);

                    // map the saved deviceOverview to deviceOverviewDto then return
                    return _mapper.Map<DeviceOverviewDTO>(deviceOverview);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Service Error: Failed to add device overview.\nError: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<DeviceOverviewDTO?> UpdateDeviceOverview(int id, UpdateDeviceOverviewDTO updateDeviceOverviewDto)
        {
            // Input Validation
            if (updateDeviceOverviewDto == null) throw new Exception("UpdateDeviceOverview failed: Input DTO is null.");
            if (string.IsNullOrWhiteSpace(updateDeviceOverviewDto.model)) throw new Exception("UpdateDeviceOverview failed: Model cannot be empty.");
            if (updateDeviceOverviewDto.device_type <= 0) throw new Exception($"UpdateDeviceOverview failed: DeviceType {updateDeviceOverviewDto.device_type} is invalid. Must be > 0.");

            try
            {
                var existingDeviceOverview = await _deviceOverviewRepository.GetDeviceOverviewById(id);

                if (existingDeviceOverview == null) throw new Exception($"UpdateDeviceOverview failed: Device with ID {id} not found.");

                // ---------------TODO : image -------------------------------------
                // if user upload a picture, call gateway for pic-handling then save it to addDeviceOverviewDto

                // -------------- Can admin change qty, available_qty direct?--------------------
                // Preserve qty and available_qty (DO NOT UPDATE)
                // int existingQty = existingDeviceOverview.qty;
                // int existingAvailableQty = existingDeviceOverview.available_qty;

                //  Update fields from DTO while keeping qty and available_qty unchanged
                _mapper.Map(updateDeviceOverviewDto, existingDeviceOverview);

                // -------------- Can admin change qty, available_qty direct?--------------------
                // existingDeviceOverview.qty = existingQty;
                // existingDeviceOverview.available_qty = existingAvailableQty;
                existingDeviceOverview.last_ordered = DateTime.UtcNow; // Update last ordered date

                // get the updated domain model
                var updatedDevciceOverview = await _deviceOverviewRepository.UpdateDeviceOverview(existingDeviceOverview);

                // return dto
                return _mapper.Map<DeviceOverviewDTO>(updatedDevciceOverview);

            }
            catch (Exception ex)
            {
                throw new Exception($"Service Error: Failed to update device overview with ID {id}.\nError: {ex.Message}");
            }
        }
    }
}
