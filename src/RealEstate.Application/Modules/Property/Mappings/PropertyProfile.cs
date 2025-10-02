using AutoMapper;
using RealEstate.Application.Modules.Property.DTOs;
using RealEstate.Domain.Modules.Property.Entities;

namespace RealEstate.Application.Modules.Property.Mappings;

public class PropertyProfile : Profile
{
    public PropertyProfile()
    {
        CreateMap<PropertyEntity, PropertyDto>()
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner));

        CreateMap<CreatePropertyDto, PropertyEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyTraces, opt => opt.Ignore());

        CreateMap<UpdatePropertyDto, PropertyEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyTraces, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
