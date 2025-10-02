using AutoMapper;
using RealEstate.Application.Modules.Property.DTOs;
using RealEstate.Application.Modules.Property.Interfaces;
using RealEstate.Domain.Modules.Property.Entities;
using RealEstate.Domain.Modules.Property.Interfaces;
using RealEstate.Domain.Modules.Property.ValueObjects;

namespace RealEstate.Application.Modules.Property.Services;

public class PropertyService(IPropertyRepository propertyRepository, IMapper mapper) : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
    {
        var properties = await _propertyRepository.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<PropertyDto?> GetPropertyByIdAsync(string id)
    {
        var property = await _propertyRepository.GetByIdWithDetailsAsync(id);
        return property is null ? null : _mapper.Map<PropertyDto>(property);
    }

    public async Task<IEnumerable<PropertyDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter)
    {
        var domainFilter = new PropertyFilter
        {
            Name = filter.Name,
            Address = filter.Address,
            MinPrice = filter.MinPrice,
            MaxPrice = filter.MaxPrice,
            Year = filter.Year,
            IdOwner = filter.IdOwner,
            CodeInternal = filter.CodeInternal
        };
        
        var properties = await _propertyRepository.GetFilteredWithDetailsAsync(domainFilter);
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<IEnumerable<PropertySimpleDto>> GetFilteredPropertiesSimpleAsync(PropertyFilterDto filter)
    {
        var domainFilter = new PropertyFilter
        {
            Name = filter.Name,
            Address = filter.Address,
            MinPrice = filter.MinPrice,
            MaxPrice = filter.MaxPrice,
            Year = filter.Year,
            IdOwner = filter.IdOwner,
            CodeInternal = filter.CodeInternal
        };
        
        var properties = await _propertyRepository.GetFilteredWithDetailsAsync(domainFilter);
        return properties.Select(p => new PropertySimpleDto
        {
            IdOwner = p.IdOwner,
            Name = p.Name,
            Address = p.Address,
            Price = p.Price,
            Image = p.GetMainImage()?.File ?? string.Empty
        });
    }

    public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto)
    {
        var property = _mapper.Map<PropertyEntity>(createPropertyDto);
        await _propertyRepository.CreateAsync(property);
        var createdProperty = await _propertyRepository.GetByIdWithDetailsAsync(property.Id);
        return _mapper.Map<PropertyDto>(createdProperty!);
    }

    public async Task UpdatePropertyAsync(string id, UpdatePropertyDto updatePropertyDto)
    {
        var existing = await _propertyRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Property with Id {id} not found.");
        
        _mapper.Map(updatePropertyDto, existing);
        await _propertyRepository.UpdateAsync(id, existing);
    }

    public async Task DeletePropertyAsync(string id)
    {
        var existing = await _propertyRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Property with Id {id} not found.");
        
        await _propertyRepository.DeleteAsync(id);
    }

    public async Task<bool> PropertyExistsAsync(string id)
    {
        var property = await _propertyRepository.GetByIdAsync(id);
        return property is not null;
    }
}
