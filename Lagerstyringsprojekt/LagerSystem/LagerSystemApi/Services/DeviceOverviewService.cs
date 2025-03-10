using AutoMapper;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;
using Microsoft.IdentityModel.Tokens;

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
            string new_file_path = "";
            // Input Validation
            if (updateDeviceOverviewDto == null)
                throw new Exception("UpdateDeviceOverview failed: Input DTO is null.");

            try
            {
                var existingDeviceOverview = await _deviceOverviewRepository.GetDeviceOverviewById(id);

                if (existingDeviceOverview == null)
                    throw new Exception($"UpdateDeviceOverview failed: Device with ID {id} not found.");

                // Preserve the old image path in case it's needed later
                string oldFilePath = existingDeviceOverview.image;

                // If a new image is uploaded, process it
                if (updateDeviceOverviewDto.image != null)
                {
                    // Save the new image and get the file path
                    string newFilePath = await _uploadImages.SaveImage(updateDeviceOverviewDto.image);
                    new_file_path = newFilePath;
                    if (newFilePath == null)
                        throw new Exception("Error saving new image");

                    // Update the image path with the new file path
                    existingDeviceOverview.image = newFilePath;

                    // Delete the old image
                    bool deleteSuccess = await _uploadImages.DeleteImage(oldFilePath);
                    if (!deleteSuccess)
                    {
                        _logger.LogWarning($"Failed to delete the old image: {oldFilePath}");
                    }
                }
                // Else, the image remains unchanged, no need to modify it

                // Update other fields (e.g., model, qty, available_qty, etc.)
                updateDeviceOverviewDto.model ??= existingDeviceOverview.model;
                updateDeviceOverviewDto.device_type = updateDeviceOverviewDto.device_type > 0 ? updateDeviceOverviewDto.device_type : existingDeviceOverview.device_type;
                updateDeviceOverviewDto.qty = updateDeviceOverviewDto.qty > 0 ? updateDeviceOverviewDto.qty : existingDeviceOverview.qty;
                updateDeviceOverviewDto.available_qty = updateDeviceOverviewDto.available_qty > 0 ? updateDeviceOverviewDto.available_qty : existingDeviceOverview.available_qty;

                // Map the DTO to the existing deviceOverview entity
                _mapper.Map(updateDeviceOverviewDto, existingDeviceOverview);

                // Update the last ordered date
                existingDeviceOverview.last_ordered = DateTime.UtcNow;

                existingDeviceOverview.image = new_file_path;

                // Ensure that the 'image' field is set properly before saving
                if (existingDeviceOverview.image == null)
                {
                    throw new Exception("Image path is null, which is not allowed.");
                }

                _logger.LogInformation($"Updating image: {existingDeviceOverview.image}");

                // Perform the update in the database
                var updatedDeviceOverview = await _deviceOverviewRepository.UpdateDeviceOverview(existingDeviceOverview);

                // Return the updated DTO
                return _mapper.Map<DeviceOverviewDTO>(updatedDeviceOverview);
            }
            catch (Exception ex)
            {
                throw new Exception($"Service Error: Failed to update device overview with ID {id}.\nError: {ex.Message}");
            }
        }
    }
}
