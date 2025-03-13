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

        private readonly IDeviceService _deviceService;

        public DeviceOverviewService(IDeviceOverviewRepository deviceOverviewRepository, IUploadImages uploadImages, ILogger<DeviceOverviewService> logger, IMapper mapper, IDeviceService deviceService)
        {
            _deviceOverviewRepository = deviceOverviewRepository;
            _uploadImages = uploadImages;
            _logger = logger;
            _mapper = mapper;
            _deviceService = deviceService;
        }

        // decrement quatity
        public async Task<bool> DecrementAvailableQuantity(int id)
        {
            return await _deviceOverviewRepository.DecrementAvailableQuantity(id);
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

                    // auto create available devices based on the quatity inserted
                    for (int i = 0; i < addDeviceOverviewDto.qty; i ++)
                    {
                        // Build an AddSingleDeviceDTO
                        var addDeviceDto = new AddSingleDeviceDTO
                        {
                            status = 1, // "available" by default
                            location = 1, // as default
                            device_overview_id = deviceOverview.id,
                            description = $"SingelDevice for {deviceOverview.model} - item { i + 1 }",
                            is_archived = false
                        };

                        // You call the service to persist the device
                        await _deviceService.AddDevice(addDeviceDto);
                        // ^ You don't store the returned DeviceDTO in a variable if you don't need it.
                    }

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
            // 1. Basic validation
            if (updateDeviceOverviewDto == null) throw new Exception("UpdateDeviceOverview failed: Input DTO is null.");
            //if (string.IsNullOrWhiteSpace(updateDeviceOverviewDto.model)) throw new Exception("UpdateDeviceOverview failed: Model cannot be empty.");
            //if (updateDeviceOverviewDto.device_type <= 0) throw new Exception($"UpdateDeviceOverview failed: DeviceType {updateDeviceOverviewDto.device_type} is invalid. Must be > 0.");

            try
            {
                // 2. Get the existing record from DB
                var existingDeviceOverview = await _deviceOverviewRepository.GetDeviceOverviewById(id);

                if (existingDeviceOverview == null) throw new Exception($"UpdateDeviceOverview failed: Device with ID {id} not found.");

                // 3. Handle optional new image upload 
                string file_path = "";
                string old_file_path = existingDeviceOverview.image; // store old path

                if (updateDeviceOverviewDto.image != null)
                {
                    // Save the new image
                    file_path = await _uploadImages.SaveImage(updateDeviceOverviewDto.image);
                    if (file_path == null) throw new Exception("Error saving new images");

                    existingDeviceOverview.image = file_path; // Update only if a new image is uploaded
                }

                // 4. Store the old quantity before mapping
                int oldQty = existingDeviceOverview.qty;
                int oldAvailableQty = existingDeviceOverview.available_qty;

                // ─────────────────────────────────────────────────────────────────────────
                // 5. PARTIAL UPDATE FALLBACK LOGIC for certain fields
                //    (We keep the old values if the DTO doesn’t provide valid ones.)
                // ─────────────────────────────────────────────────────────────────────────
                // If the user doesn't supply a 'model', keep the existing one.
                updateDeviceOverviewDto.model ??= existingDeviceOverview.model;

                // If the user sets 'device_type' to 0 or less, keep the old one.
                updateDeviceOverviewDto.device_type = updateDeviceOverviewDto.device_type > 0 ? updateDeviceOverviewDto.device_type : existingDeviceOverview.device_type;
                updateDeviceOverviewDto.available_qty = updateDeviceOverviewDto.available_qty > 0 ? updateDeviceOverviewDto.available_qty : existingDeviceOverview.available_qty;

                //     NOTE: We do NOT do fallback for 'qty' here because we treat it
                //       as "the number of new items to add" (explained below).
                //       If the user doesn't provide it (or it's zero),
                //       we won't add any new SingleDevices.
                // updateDeviceOverviewDto.qty = updateDeviceOverviewDto.qty > 0 ? updateDeviceOverviewDto.qty : existingDeviceOverview.qty;

                
                Console.WriteLine("Image before: " + existingDeviceOverview.image);

                // 6. Now map the (cleaned-up) DTO onto the existing entity
                _mapper.Map(updateDeviceOverviewDto, existingDeviceOverview);

                // 7. If we have a new image, replace the old one
                if (!file_path.IsNullOrEmpty())
                {
                    existingDeviceOverview.image = file_path;
                    // Deletes old image, if no errors occur
                    bool delete = await _uploadImages.DeleteImage(existingDeviceOverview.image);

                    if (!delete) throw new Exception("Error while deleting image");
                }
                else
                {
                    existingDeviceOverview.image = old_file_path;
                }
                Console.WriteLine("Image after: " + existingDeviceOverview.image);

                // ─────────────────────────────────────────────────────────────────────────
                // 8. Interpret 'updateDeviceOverviewDto.qty' as "add more items"
                //    If the user says qty=5, we add 5 to the old total and create 5 new SingleDevices.
                // ─────────────────────────────────────────────────────────────────────────

                //  if qty is null, treat it as 0 (i.e., “no additional items”).
                int qtyToAdd = updateDeviceOverviewDto.qty ?? 0;  // This is the increment
                if (qtyToAdd > 0)
                {
                    // Increase the total by 'qtyToAdd'
                    existingDeviceOverview.qty = oldQty + qtyToAdd;
                    existingDeviceOverview.available_qty = oldAvailableQty + qtyToAdd;

                    // Create 'qtyToAdd' new SingleDevice records
                    for (int i = 0; i < qtyToAdd; i++)
                    {
                        var addDeviceDto = new AddSingleDeviceDTO
                        {
                            status = 1,   // e.g., "available"
                            location = 1, // or pass from your DTO if you want partial updates for location
                            device_overview_id = existingDeviceOverview.id,
                            description = $"SingleDevice for {existingDeviceOverview.model} - item {oldQty + i + 1}",
                            is_archived = false
                        };

                        // Usedevice service to create each SingleDevice
                        await _deviceService.AddDevice(addDeviceDto);
                    }
                }
                else
                {
                    // If qty <= 0, do nothing (no new devices).
                }


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
