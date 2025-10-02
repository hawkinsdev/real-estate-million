using RealEstate.Domain.Common.Interfaces;
using RealEstate.Domain.Modules.Property.Entities;

namespace RealEstate.Domain.Modules.Property.Interfaces
{
    public interface IPropertyImageRepository : IAsyncQueryableRepository<PropertyImage>
    {
        Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(string propertyId, bool enabledOnly = true);
        Task<PropertyImage?> GetMainImageByPropertyIdAsync(string propertyId);
        Task DeleteByPropertyIdAsync(string propertyId);
        Task<int> GetImageCountByPropertyIdAsync(string propertyId, bool enabledOnly = true);
        Task EnableImageAsync(string imageId);
        Task DisableImageAsync(string imageId);
        Task<IEnumerable<PropertyImage>> GetEnabledImagesAsync();
        Task<IEnumerable<PropertyImage>> GetDisabledImagesAsync();
        Task BulkEnableByPropertyIdAsync(string propertyId);
        Task BulkDisableByPropertyIdAsync(string propertyId);
    }
}