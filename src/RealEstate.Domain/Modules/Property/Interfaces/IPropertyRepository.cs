using RealEstate.Domain.Common.Interfaces;
using RealEstate.Domain.Modules.Property.Entities;

namespace RealEstate.Domain.Modules.Property.Interfaces
{
    public interface IPropertyRepository : IAsyncQueryableRepository<PropertyEntity>
    {
        Task<IEnumerable<PropertyEntity>> GetByOwnerIdAsync(string ownerId);
        Task<IEnumerable<PropertyEntity>> GetFilteredAsync(
            string? name = null,
            string? address = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? year = null,
            string? ownerId = null);
        Task<PropertyEntity?> GetByCodeInternalAsync(string codeInternal);
        Task<IEnumerable<PropertyEntity>> GetPropertiesWithDetailsAsync();
        Task<PropertyEntity?> GetWithDetailsAsync(string id);
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