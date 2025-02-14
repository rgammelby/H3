using AutoMapper;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;

namespace LagerSystemApi.Services
{
    public class StatusTypeService : IStatusTypeService
    {
        private readonly IStatusTypeRepository _statusTypeRepository;
        private readonly IMapper _mapper;
        
        public StatusTypeService(IStatusTypeRepository statusType, IMapper mapper)
        {
            _statusTypeRepository = statusType;
            _mapper = mapper;
        }
        
        public async Task<List<StatusTypeDTO>> GetAllStatusTypes()
        {
            var statusTypes = await _statusTypeRepository.GetAllStatusTypes();
            
            // Ensure proper mapping from StatusType to StatusTypeDTO
            var statusTypeDTOs = _mapper.Map<List<StatusTypeDTO>>(statusTypes);
            return statusTypeDTOs;
        }
    }
}
