using RealEstate.Domain.Common.Interfaces;
using RealEstate.Domain.Modules.Property.Entities;
using RealEstate.Domain.Modules.Property.ValueObjects;

namespace RealEstate.Domain.Modules.Property.Interfaces
{
    public interface IPropertyRepository : IAsyncQueryableRepository<PropertyEntity>
    {
        Task<IEnumerable<PropertyEntity>> GetByOwnerIdAsync(string ownerId);
        Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(string propertyId);
        Task<IEnumerable<PropertyEntity>> GetFilteredAsync(
            string? name = null,
            string? address = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? year = null,
            string? ownerId = null);
        Task<IEnumerable<PropertyEntity>> GetFilteredWithDetailsAsync(PropertyFilter filter);
        Task<IEnumerable<PropertyEntity>> GetFilteredSimpleAsync(PropertyFilter filter);
        Task<PropertyEntity?> GetByCodeInternalAsync(string codeInternal);
        Task<IEnumerable<PropertyEntity>> GetPropertiesWithDetailsAsync();
        Task<IEnumerable<PropertyEntity>> GetAllWithDetailsAsync();
        Task<PropertyEntity?> GetWithDetailsAsync(string id);
        Task<PropertyEntity?> GetByIdWithDetailsAsync(string id);
        Task<IEnumerable<PropertyEntity>> GetPropertiesByYearRangeAsync(int minYear, int maxYear);
        Task<IEnumerable<PropertyEntity>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<PropertyEntity>> SearchPropertiesAsync(string searchTerm);
        Task<bool> CodeInternalExistsAsync(string codeInternal, string? excludeId = null);
        Task<decimal> GetAveragePriceAsync();
        Task<decimal> GetAveragePriceByOwnerAsync(string ownerId);
        Task<IEnumerable<PropertyEntity>> GetVintagePropertiesAsync();
        Task<IEnumerable<PropertyEntity>> GetRecentPropertiesAsync(int days = 30);
    }
}