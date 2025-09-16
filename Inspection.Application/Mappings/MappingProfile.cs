using AutoMapper;
using Inspection.Application.Dto;
using Inspection.Domain.Entities;

namespace Inspection.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser,UserDto>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ForMember(dest => dest.RoleName, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<CreateUserDto, ApplicationUser>()
                .ReverseMap();
            CreateMap<UpdateUserDto, ApplicationUser>()
                .ReverseMap();

            CreateMap<ApplicationRole, RoleDto>()
                .ReverseMap();

            CreateMap<Inspector, InspectorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<CreateInspectorDto, Inspector>()
                                               .ReverseMap();

            CreateMap<UpdateInspectorDto, Inspector>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            CreateMap<EntityToInspect, EntityToInspectDto>();
            CreateMap<CreateEntityToInspectDto, EntityToInspect>();
            CreateMap<UpdateEntityToInspectDto, EntityToInspect>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            CreateMap<InspectionVisit, InspectionVisitDto>();
            CreateMap<CreateInspectionVisitDto, InspectionVisit>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enum.InspectionStatus.Planned));
            CreateMap<UpdateInspectionVisitDto, InspectionVisit>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            CreateMap<Violation, ViolationDto>();
            CreateMap<CreateViolationDto, Violation>();
            CreateMap<UpdateViolationDto, Violation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InspectionVisitId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            CreateMap<InspectionVisit, RecentVisitDto>()
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.EntityToInspect.Name))
                .ForMember(dest => dest.InspectorName, opt => opt.MapFrom(src => src.Inspector.User.FullName));
        }
    }
}
