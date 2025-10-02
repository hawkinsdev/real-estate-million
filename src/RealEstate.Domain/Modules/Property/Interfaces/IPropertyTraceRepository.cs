using RealEstate.Domain.Common.Interfaces;
using RealEstate.Domain.Modules.Property.Entities;

namespace RealEstate.Domain.Modules.Property.Interfaces
{
    public interface IPropertyTraceRepository : IAsyncQueryableRepository<PropertyTrace>
    {
        Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(string propertyId);
        Task<IEnumerable<PropertyTrace>> GetTracesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<PropertyTrace>> GetTracesByValueRangeAsync(decimal minValue, decimal maxValue);
        Task DeleteByPropertyIdAsync(string propertyId);
        Task<PropertyTrace?> GetLatestTraceByPropertyIdAsync(string propertyId);
        Task<IEnumerable<PropertyTrace>> GetRecentSalesAsync(int days = 30);
        Task<IEnumerable<PropertyTrace>> GetFutureSalesAsync();
        Task<decimal> GetAverageValueAsync();
        Task<decimal> GetAverageValueByPropertyIdAsync(string propertyId);
        Task<decimal> GetTotalTaxesAsync();
        Task<decimal> GetTotalTaxesByPropertyIdAsync(string propertyId);
        Task<IEnumerable<PropertyTrace>> GetHighValueSalesAsync(decimal threshold);
        Task<int> GetSalesCountByPropertyIdAsync(string propertyId);
        Task<IEnumerable<PropertyTrace>> GetSalesByOwnerAsync(string ownerId);
    }
}