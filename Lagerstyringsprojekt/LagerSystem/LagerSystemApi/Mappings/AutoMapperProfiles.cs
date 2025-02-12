using AutoMapper;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            // Map between DeviceOverview and DeviceOverviewDTO
            // DeviceOverviewDTO: Used for retrieving data.
            CreateMap<DeviceOverview, DeviceOverviewDTO>().ReverseMap();

            // AddDeviceOverviewDTO: Used for creating a new device
            CreateMap<DeviceOverview, AddDeviceOverviewDTO>().ReverseMap();

            // UpdateDeviceOverviewDTO: Used for updating an existing device
            CreateMap<DeviceOverview, UpdateDeviceOverviewDTO>().ReverseMap();

            // Map between Device and DeviceDTO
            // DeviceDTO: Used for retrieving data.
            CreateMap<DeviceDTO, SingleDevice>().ReverseMap();

            //Mapping for Adding a New Device (DTO → Domain)
            CreateMap<AddSingleDeviceDTO, SingleDevice>()
                .ForMember(dest => dest.Location, opt => opt.Ignore());  //  Ignore Navigation Property
            
            // UpdateDeviceDTO: Used for updating an existing device
            CreateMap<UpdateDeviceDTO, SingleDevice>()
                .ForMember(dest => dest.Location, opt => opt.Ignore());  //  Ignore Navigation Property
            
            // only mapping from Log (domain model) to LogDTO (Data Transfer Object).
            CreateMap<Log, LogDTO>();
        }
    }
}
