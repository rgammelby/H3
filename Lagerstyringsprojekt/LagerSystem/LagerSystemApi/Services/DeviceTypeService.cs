using AutoMapper;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Services
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly IDeviceTypeRepository _deviceTypeRepository;
        private readonly IMapper _mapper;

        public DeviceTypeService(IDeviceTypeRepository deviceTypeRepository, IMapper mapper)
        {
            _deviceTypeRepository = deviceTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<DeviceTypeDTO>> GetAllDeviceTypes()
        {
            var deviceTypes = await _deviceTypeRepository.GetAllDeviceTypes();

            // OBS mismatch id and name in DeviceTypeDTO
            var deviceTypeDtos =  _mapper.Map<List<DeviceTypeDTO>>(deviceTypes);
            return deviceTypeDtos;
        }
    }
}
