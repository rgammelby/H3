using AutoMapper;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<List<LocationCupboardDTO>> GetAllCupboards()
        {
            var cupboards = await _locationRepository.GetAllCupboards();

            // Ensure proper mapping from StatusType to StatusTypeDTO
            var cupboardDTOs = _mapper.Map<List<LocationCupboardDTO>>(cupboards);
            return cupboardDTOs;
        }

        public async Task<List<LocationRoomDTO>> GetAllRooms()
        {
            var rooms = await _locationRepository.GetAllRooms();

            // Ensure proper mapping from StatusType to StatusTypeDTO
            var roomDTOs = _mapper.Map<List<LocationRoomDTO>>(rooms);
            return roomDTOs;
        }
    }
}
