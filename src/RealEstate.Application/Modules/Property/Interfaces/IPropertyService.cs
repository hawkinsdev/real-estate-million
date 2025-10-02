using RealEstate.Application.Modules.Property.DTOs;

namespace RealEstate.Application.Modules.Property.Interfaces;

public interface IPropertyService
{
    Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
    Task<PropertyDto?> GetPropertyByIdAsync(string id);
    Task<IEnumerable<PropertyDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter);
    Task<IEnumerable<PropertySimpleDto>> GetFilteredPropertiesSimpleAsync(PropertyFilterDto filter);
    Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto);
    Task UpdatePropertyAsync(string id, UpdatePropertyDto updatePropertyDto);
    Task DeletePropertyAsync(string id);
    Task<bool> PropertyExistsAsync(string id);
}
