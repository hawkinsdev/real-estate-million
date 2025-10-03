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
        // Use basic method that works
        var properties = await _propertyRepository.GetAllAsync();
        var propertyDtos = _mapper.Map<IEnumerable<PropertyDto>>(properties);
        
        // Add images to each property
        foreach (var propertyDto in propertyDtos)
        {
            propertyDto.Images = new List<PropertyImageDto>
            {
                new PropertyImageDto
                {
                    IdPropertyImage = $"mock-image-1-{propertyDto.IdProperty}",
                    IdProperty = propertyDto.IdProperty,
                    File = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&h=600&fit=crop",
                    Enabled = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new PropertyImageDto
                {
                    IdPropertyImage = $"mock-image-2-{propertyDto.IdProperty}",
                    IdProperty = propertyDto.IdProperty,
                    File = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800&h=600&fit=crop",
                    Enabled = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }
        
        return propertyDtos;
    }

    public async Task<PropertyDto?> GetPropertyByIdAsync(string id)
    {
        var property = await _propertyRepository.GetByIdAsync(id);
        if (property is null) return null;
        
        var propertyDto = _mapper.Map<PropertyDto>(property);
        
        // Add images to the property
        propertyDto.Images = new List<PropertyImageDto>
        {
            new PropertyImageDto
            {
                IdPropertyImage = $"mock-image-1-{propertyDto.IdProperty}",
                IdProperty = propertyDto.IdProperty,
                File = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&h=600&fit=crop",
                Enabled = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new PropertyImageDto
            {
                IdPropertyImage = $"mock-image-2-{propertyDto.IdProperty}",
                IdProperty = propertyDto.IdProperty,
                File = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800&h=600&fit=crop",
                Enabled = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        
        return propertyDto;
    }

    public async Task<IEnumerable<PropertyDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter)
    {
        // Get all properties first
        var allProperties = await _propertyRepository.GetAllAsync();
        var propertyDtos = _mapper.Map<IEnumerable<PropertyDto>>(allProperties);
        
        // Add images to each property
        foreach (var propertyDto in propertyDtos)
        {
            propertyDto.Images = new List<PropertyImageDto>
            {
                new PropertyImageDto
                {
                    IdPropertyImage = $"mock-image-1-{propertyDto.IdProperty}",
                    IdProperty = propertyDto.IdProperty,
                    File = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&h=600&fit=crop",
                    Enabled = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new PropertyImageDto
                {
                    IdPropertyImage = $"mock-image-2-{propertyDto.IdProperty}",
                    IdProperty = propertyDto.IdProperty,
                    File = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800&h=600&fit=crop",
                    Enabled = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        // Apply filters in memory
        if (!string.IsNullOrEmpty(filter.Name))
        {
            propertyDtos = propertyDtos.Where(p => p.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(filter.Address))
        {
            propertyDtos = propertyDtos.Where(p => p.Address.Contains(filter.Address, StringComparison.OrdinalIgnoreCase));
        }

        if (filter.MinPrice.HasValue)
        {
            propertyDtos = propertyDtos.Where(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            propertyDtos = propertyDtos.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        if (filter.Year.HasValue)
        {
            propertyDtos = propertyDtos.Where(p => p.Year == filter.Year.Value);
        }

        if (!string.IsNullOrEmpty(filter.IdOwner))
        {
            propertyDtos = propertyDtos.Where(p => p.IdOwner == filter.IdOwner);
        }

        if (!string.IsNullOrEmpty(filter.CodeInternal))
        {
            propertyDtos = propertyDtos.Where(p => p.CodeInternal == filter.CodeInternal);
        }

        return propertyDtos;
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
        
        // Use full method with aggregation to get complete information
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
