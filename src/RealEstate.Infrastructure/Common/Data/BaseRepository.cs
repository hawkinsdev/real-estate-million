using System.Linq.Expressions;
using MongoDB.Driver;
using RealEstate.Domain.Modules.Property.Entities;
using RealEstate.Domain.Modules.Property.Interfaces;

namespace RealEstate.Infrastructure.Common.Data
{
    public class PropertyRepository(MongoDbContext context) : IPropertyRepository
    {
        private readonly IMongoCollection<PropertyEntity> _properties = context.Properties ?? throw new ArgumentNullException(nameof(context));

        public async Task<IEnumerable<PropertyEntity>> GetAllAsync()
        {
            try
            {
                return await _properties
                    .Find(_ => true)
                    .SortByDescending(p => p.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving properties from database", ex);
            }
        }

        public async Task<PropertyEntity?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be null or empty", nameof(id));

            try
            {
                return await _properties
                    .Find(p => p.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving property with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<PropertyEntity>> GetFilteredAsync(
            string? name = null,
            string? address = null,
            decimal? minPrice = null,
            decimal? maxPrice = null)
        {
            try
            {
                var filterBuilder = Builders<PropertyEntity>.Filter;
                var filter = filterBuilder.Empty;

                if (!string.IsNullOrWhiteSpace(name))
                {
                    filter &= filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
                }

                if (!string.IsNullOrWhiteSpace(address))
                {
                    filter &= filterBuilder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(address, "i"));
                }

                if (minPrice.HasValue)
                {
                    filter &= filterBuilder.Gte(p => p.Price, minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    filter &= filterBuilder.Lte(p => p.Price, maxPrice.Value);
                }

                return await _properties
                    .Find(filter)
                    .SortByDescending(p => p.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error filtering properties", ex);
            }
        }

        public async Task<PropertyEntity> CreateAsync(PropertyEntity property)
        {
            ArgumentNullException.ThrowIfNull(property);

            try
            {
                property.CreatedAt = DateTime.UtcNow;
                property.UpdatedAt = DateTime.UtcNow;

                await _properties.InsertOneAsync(property);
                return property;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating property", ex);
            }
        }

        public async Task UpdateAsync(string id, PropertyEntity property)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be null or empty", nameof(id));

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            try
            {
                property.UpdatedAt = DateTime.UtcNow;

                var result = await _properties.ReplaceOneAsync(p => p.Id == id, property);

                if (result.MatchedCount == 0)
                    throw new KeyNotFoundException($"Property with ID {id} not found");
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating property with ID {id}", ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be null or empty", nameof(id));

            try
            {
                var result = await _properties.DeleteOneAsync(p => p.Id == id);

                if (result.DeletedCount == 0)
                    throw new KeyNotFoundException($"Property with ID {id} not found");
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting property with ID {id}", ex);
            }
        }

        public async Task<bool> ExistsAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            try
            {
                var count = await _properties.CountDocumentsAsync(p => p.Id == id);
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking if property with ID {id} exists", ex);
            }
        }

        public Task<IEnumerable<PropertyEntity>> GetByOwnerIdAsync(string ownerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> GetFilteredAsync(string? name = null, string? address = null, decimal? minPrice = null, decimal? maxPrice = null, int? year = null, string? ownerId = null)
        {
            throw new NotImplementedException();
        }

        public Task<PropertyEntity?> GetByCodeInternalAsync(string codeInternal)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> GetPropertiesWithDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PropertyEntity?> GetWithDetailsAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> GetPropertiesByYearRangeAsync(int minYear, int maxYear)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> SearchPropertiesAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CodeInternalExistsAsync(string codeInternal, string? excludeId = null)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetAveragePriceAsync()
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetAveragePriceByOwnerAsync(string ownerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> GetVintagePropertiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> GetRecentPropertiesAsync(int days = 30)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> ProjectAsync<TResult>(Expression<Func<PropertyEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> ProjectAsync<TResult>(Expression<Func<PropertyEntity, bool>> predicate, Expression<Func<PropertyEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<PropertyEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<PropertyEntity?> FirstOrDefaultAsync(Expression<Func<PropertyEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<PropertyEntity> FirstAsync(Expression<Func<PropertyEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> FindAsync(Expression<Func<PropertyEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<PropertyEntity?> FindOneAsync(Expression<Func<PropertyEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> GetPagedAsync(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyEntity>> GetPagedAsync(Expression<Func<PropertyEntity, bool>> predicate, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(Expression<Func<PropertyEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync()
        {
            throw new NotImplementedException();
        }
    }
}