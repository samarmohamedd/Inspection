using AutoMapper;
using Inspection.Application.Dto;
using Inspection.Domain.Entities;

namespace Inspection.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings

            CreateMap<UserDto, ApplicationUser>()
                .ReverseMap();
            CreateMap<CreateUserDto, ApplicationUser>()
                .ReverseMap();
            CreateMap<UpdateUserDto, ApplicationUser>()
                .ReverseMap();

            CreateMap<Inspector, InspectorDto>()
                                .ReverseMap();

            CreateMap<CreateInspectorDto, Inspector>()
                                               .ReverseMap();

            CreateMap<UpdateInspectorDto, Inspector>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            // EntityToInspect mappings
            CreateMap<EntityToInspect, EntityToInspectDto>();
            CreateMap<CreateEntityToInspectDto, EntityToInspect>();
            CreateMap<UpdateEntityToInspectDto, EntityToInspect>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            // InspectionVisit mappings
            CreateMap<InspectionVisit, InspectionVisitDto>();
            CreateMap<CreateInspectionVisitDto, InspectionVisit>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enum.InspectionStatus.Planned));
            CreateMap<UpdateInspectionVisitDto, InspectionVisit>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            // Violation mappings
            CreateMap<Violation, ViolationDto>();
            CreateMap<CreateViolationDto, Violation>();
            CreateMap<UpdateViolationDto, Violation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InspectionVisitId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            // Recent visit mapping
            CreateMap<InspectionVisit, RecentVisitDto>()
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.EntityToInspect.Name))
                .ForMember(dest => dest.InspectorName, opt => opt.MapFrom(src => src.Inspector.User.FullName));
        }
    }
}
