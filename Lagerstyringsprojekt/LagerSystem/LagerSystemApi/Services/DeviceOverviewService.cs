using AutoMapper;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;
using Microsoft.Extensions.Logging;

namespace LagerSystemApi.Services
{
    public class DeviceOverviewService : IDeviceOverviewService
    {
        
        private readonly IDeviceOverviewRepository _deviceOverviewRepository;
        private readonly ILogger<DeviceOverviewService> _logger;
        private readonly IMapper _mapper;

        public DeviceOverviewService(IDeviceOverviewRepository deviceOverviewRepository, ILogger<DeviceOverviewService> logger, IMapper mapper)
        {
            _deviceOverviewRepository = deviceOverviewRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<DeviceOverviewDTO?> GetDeviceOverviewById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"GetDeviceOverviewById failed: Invalid ID {id}.");
                return null;
            }
            try
            {
                var deviceOverview = await _deviceOverviewRepository.GetDeviceOverviewById(id);

                if (deviceOverview == null)
                {
                    _logger.LogWarning($"DeviceOverview with id {id} not found.");
                    return null;
                }

                return _mapper.Map<DeviceOverviewDTO>(deviceOverview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service Error: Failed to retrieve device overview with ID {id}");
                throw;
            }
        }
        public async Task<List<DeviceOverviewDTO>> GetAllDeviceOverviews()
        {
            try
            {
                var deviceOverviews = await _deviceOverviewRepository.GetAllDeviceOverviews();

                return _mapper.Map<List<DeviceOverviewDTO>>(deviceOverviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service Error: Failed to retrieve all device overviews");
                throw;
            }
        }
        public async Task<DeviceOverviewDTO?> AddDeviceOverview(AddDeviceOverviewDTO addDeviceOverviewDto)
        {
            // Validate input fields (e.g., device_type > 1, model is not empty).
            if (addDeviceOverviewDto == null)
            {
                _logger.LogError("AddDeviceOverview failed: Input DTO is null.");
                return null;
            }

            if (string.IsNullOrWhiteSpace(addDeviceOverviewDto.model))
            {
                _logger.LogError("AddDeviceOverview failed: Model cannot be empty.");
                return null;
            }

            if (addDeviceOverviewDto.device_type <= 1)
            {
                _logger.LogError($"AddDeviceOverview failed: DeviceType {addDeviceOverviewDto.device_type} is invalid. Must be > 1.");
                return null;
            }

            if (addDeviceOverviewDto.qty < 0 || addDeviceOverviewDto.available_qty < 0)
            {
                _logger.LogError($"AddDeviceOverview failed: Qty {addDeviceOverviewDto.qty} and AvailableQty {addDeviceOverviewDto.available_qty} must be >= 0.");
                return null;
            }


            try
            {
                // 1. Check if a device with the same `model` and `device_type` already exists
                var existingDeviceOverview = await _deviceOverviewRepository.GetDeviceOverviewByModelAndType(addDeviceOverviewDto.model, addDeviceOverviewDto.device_type);

                if (existingDeviceOverview != null)
                {
                    _logger.LogInformation($"DeviveOverview with Model {addDeviceOverviewDto?.model} and DeviceType {addDeviceOverviewDto.device_type} already exists. Updating instead of creating new.");

                    // Map new data from DTO while keeping the same ID
                    _mapper.Map(addDeviceOverviewDto, existingDeviceOverview);
                    existingDeviceOverview.last_ordered = DateTime.UtcNow; // Update last ordered date

                    var updatedDeviceOverview = await _deviceOverviewRepository.UpdateDeviceOverview(existingDeviceOverview);

                    // return dto of updated overview
                    return _mapper.Map<DeviceOverviewDTO>(updatedDeviceOverview);
                }

                // ---------------TODO : image -------------------------------------
                // if user upload a picture, call gateway for pic-handling then save it to addDeviceOverviewDto


                var deviceOverview = _mapper.Map<DeviceOverview>(addDeviceOverviewDto);

                //  save deviceOverview to db
                await _deviceOverviewRepository.AddDeviceOverview(deviceOverview);

                // map the saved deviceOverview to deviceOverviewDto then return
                return _mapper.Map<DeviceOverviewDTO>(deviceOverview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service Error: Failed to add device overview");
                throw;
            }
        }
        public async Task<DeviceOverviewDTO?> UpdateDeviceOverview(UpdateDeviceOverviewDTO updateDeviceOverviewDto)
        {
            // Input Validation
            if (updateDeviceOverviewDto == null)
            {
                _logger.LogError("UpdateDeviceOverview failed: Input DTO is null.");
                return null;
            }

            if (updateDeviceOverviewDto.id <= 0)
            {
                _logger.LogError($"UpdateDeviceOverview failed: Invalid ID {updateDeviceOverviewDto.id}.");
                return null;
            }
            if (string.IsNullOrWhiteSpace(updateDeviceOverviewDto.model))
            {
                _logger.LogError("UpdateDeviceOverview failed: Model cannot be empty.");
                return null;
            }

            if (updateDeviceOverviewDto.device_type <= 0)
            {
                _logger.LogError($"UpdateDeviceOverview failed: DeviceType {updateDeviceOverviewDto.device_type} is invalid. Must be > 0.");
                return null;
            }

            try
            {
                var existingDeviceOverview = await _deviceOverviewRepository.GetDeviceOverviewById(updateDeviceOverviewDto.id);

                if (existingDeviceOverview == null)
                {
                    _logger.LogError($"UpdateDeviceOverview failed: Device with ID {updateDeviceOverviewDto.id} not found.");
                    return null;
                }

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
                _logger.LogError(ex, $"Service Error: Failed to update device overview with ID {updateDeviceOverviewDto.id}");
                throw;
            }
        }
    }
}
