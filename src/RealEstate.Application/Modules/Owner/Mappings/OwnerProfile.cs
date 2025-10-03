using AutoMapper;
using RealEstate.Application.Modules.Owner.DTOs;
using RealEstate.Domain.Modules.Owner.Entities;

namespace RealEstate.Application.Modules.Owner.Mappings
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<OwnerEntity, OwnerDto>()
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdOwner));

            CreateMap<OwnerEntity, OwnerWithPropertiesDto>()
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.GetAge()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties));

            CreateMap<CreateOwnerDto, OwnerEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateOwnerDto, OwnerEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}