using RealEstate.Domain.Common.Interfaces;
using RealEstate.Domain.Modules.Owner.Entities;

namespace RealEstate.Domain.Modules.Owner.Interfaces
{
    public interface IOwnerRepository : IAsyncQueryableRepository<OwnerEntity>
    {
        Task<IEnumerable<OwnerEntity>> GetOwnersByAgeRangeAsync(int minAge, int maxAge);
        Task<IEnumerable<OwnerEntity>> SearchOwnersByNameAsync(string searchTerm);
        Task<OwnerEntity?> GetOwnerWithPropertiesAsync(string id);
        Task<IEnumerable<OwnerEntity>> GetOwnersWithPropertiesAsync();
        Task<bool> HasPropertiesAsync(string ownerId);
        Task<int> GetPropertyCountAsync(string ownerId);
        Task<IEnumerable<OwnerEntity>> GetOwnersByBirthdayMonthAsync(int month);
    }
}