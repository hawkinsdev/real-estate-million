using RealEstate.Application.Modules.Owner.DTOs;

namespace RealEstate.Application.Modules.Owner.Interfaces;

public interface IOwnerService
{
    Task<IEnumerable<OwnerDto>> GetAllOwnersAsync();
    Task<IEnumerable<OwnerWithPropertiesDto>> GetAllOwnersWithPropertiesAsync();
    Task<OwnerDto?> GetOwnerByIdAsync(string id);
    Task<OwnerWithPropertiesDto?> GetOwnerWithPropertiesByIdAsync(string id);
    Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto);
    Task UpdateOwnerAsync(string id, UpdateOwnerDto updateOwnerDto);
    Task DeleteOwnerAsync(string id);
    Task<bool> OwnerExistsAsync(string id);
}