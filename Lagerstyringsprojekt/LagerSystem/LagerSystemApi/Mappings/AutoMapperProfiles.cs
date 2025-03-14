using AutoMapper;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {

            // Map between ActivityDTO and Activity
            CreateMap<ActivityDTO, Activity>();

            // Map between DeviceOverview and DeviceOverviewDTO
            // DeviceOverviewDTO: Used for retrieving data.
            CreateMap<DeviceOverview, DeviceOverviewDTO>().ReverseMap();

            // AddDeviceOverviewDTO: Used for creating a new device
            CreateMap<AddDeviceOverviewDTO, DeviceOverview>()
                .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.image_path)) // Map ImagePath → Image
                .ReverseMap()
                .ForMember(dest => dest.image_path, opt => opt.MapFrom(src => src.image)); // Map back

            // UpdateDeviceOverviewDTO: Used for updating an existing device
            CreateMap<UpdateDeviceOverviewDTO, DeviceOverview>()
                .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.image_path)) // Ensure image_path → image
                .ReverseMap()
                .ForMember(dest => dest.image_path, opt => opt.MapFrom(src => src.image)); // Ensure image → image_path

            CreateMap<DeviceDTO, SingleDevice>().ReverseMap();

            //CreateMap<AddSingleDeviceDTO, SingleDevice>().ReverseMap();
            //Mapping for Adding a New Device (DTO → Domain)
            CreateMap<AddSingleDeviceDTO, SingleDevice>()
                .ForMember(dest => dest.Location, opt => opt.Ignore());  //  Ignore Navigation Property

            CreateMap<UpdateDeviceDTO, SingleDevice>()
                .ForMember(dest => dest.Location, opt => opt.Ignore());  //  Ignore Navigation Property

             // only mapping from Log (domain model) to LogDTO (Data Transfer Object).
             CreateMap<Log, LogDTO>();

            // retrieve status type data
            CreateMap<StatusType, StatusTypeDTO>().ReverseMap();
            // retrieve location cupboard data
            CreateMap<LocationCupboard, LocationCupboardDTO>().ReverseMap();
            // retrieve location room data
            CreateMap<LocationRoom, LocationRoomDTO>().ReverseMap();

            // Mapping deviceType and DTO
            CreateMap<DeviceType, DeviceTypeDTO>().ReverseMap();
             
        }
    }
}
