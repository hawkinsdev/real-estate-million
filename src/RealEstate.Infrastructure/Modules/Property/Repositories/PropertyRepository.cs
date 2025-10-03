using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Domain.Modules.Owner.Entities;
using RealEstate.Domain.Modules.Property.Entities;
using RealEstate.Domain.Modules.Property.Interfaces;
using RealEstate.Domain.Modules.Property.ValueObjects;
using RealEstate.Infrastructure.Common.Data;

namespace RealEstate.Infrastructure.Modules.Property.Repositories;

public class PropertyRepository(MongoDbContext context) : IPropertyRepository
{
    private readonly IMongoCollection<PropertyEntity> _properties = context.Properties;
    private readonly IMongoCollection<OwnerEntity> _owners = context.Owners;
    private readonly IMongoCollection<PropertyImage> _images = context.Database.GetCollection<PropertyImage>("PropertyImages");

    public async Task<IEnumerable<PropertyEntity>> GetAllAsync()
    {
        return await _properties.Find(_ => true).ToListAsync();
    }

    public async Task<PropertyEntity?> GetByIdAsync(string id)
    {
        return await _properties.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(string propertyId)
    {
        return await _images.Find(img => img.IdProperty == propertyId).ToListAsync();
    }

    public async Task<PropertyEntity> CreateAsync(PropertyEntity entity)
    {
        await _properties.InsertOneAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(string id, PropertyEntity entity)
    {
        await _properties.ReplaceOneAsync(p => p.Id == id, entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _properties.DeleteOneAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var count = await _properties.CountDocumentsAsync(p => p.Id == id);
        return count > 0;
    }

    public async Task<IEnumerable<PropertyEntity>> GetByOwnerIdAsync(string ownerId)
    {
        return await _properties.Find(p => p.IdOwner == ownerId).ToListAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetFilteredAsync(
        string? name = null,
        string? address = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? year = null,
        string? ownerId = null)
    {
        var filter = Builders<PropertyEntity>.Filter.Empty;

        if (!string.IsNullOrEmpty(name))
            filter &= Builders<PropertyEntity>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

        if (!string.IsNullOrEmpty(address))
            filter &= Builders<PropertyEntity>.Filter.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(address, "i"));

        if (minPrice.HasValue)
            filter &= Builders<PropertyEntity>.Filter.Gte(p => p.Price, minPrice.Value);

        if (maxPrice.HasValue)
            filter &= Builders<PropertyEntity>.Filter.Lte(p => p.Price, maxPrice.Value);

        if (year.HasValue)
            filter &= Builders<PropertyEntity>.Filter.Eq(p => p.Year, year.Value);

        if (!string.IsNullOrEmpty(ownerId))
            filter &= Builders<PropertyEntity>.Filter.Eq(p => p.IdOwner, ownerId);

        return await _properties.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetFilteredSimpleAsync(PropertyFilter filter)
    {
        var mongoFilter = Builders<PropertyEntity>.Filter.Empty;

        if (!string.IsNullOrEmpty(filter.Name))
            mongoFilter &= Builders<PropertyEntity>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(filter.Name, "i"));

        if (!string.IsNullOrEmpty(filter.Address))
            mongoFilter &= Builders<PropertyEntity>.Filter.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(filter.Address, "i"));

        if (filter.MinPrice.HasValue)
            mongoFilter &= Builders<PropertyEntity>.Filter.Gte(p => p.Price, filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            mongoFilter &= Builders<PropertyEntity>.Filter.Lte(p => p.Price, filter.MaxPrice.Value);

        if (filter.Year.HasValue)
            mongoFilter &= Builders<PropertyEntity>.Filter.Eq(p => p.Year, filter.Year.Value);

        if (!string.IsNullOrEmpty(filter.IdOwner))
            mongoFilter &= Builders<PropertyEntity>.Filter.Eq(p => p.IdOwner, filter.IdOwner);

        if (!string.IsNullOrEmpty(filter.CodeInternal))
            mongoFilter &= Builders<PropertyEntity>.Filter.Eq(p => p.CodeInternal, filter.CodeInternal);

        return await _properties.Find(mongoFilter).ToListAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetFilteredWithDetailsAsync(PropertyFilter filter)
    {
        var mongoFilter = Builders<PropertyEntity>.Filter.Empty;

        if (!string.IsNullOrEmpty(filter.Name))
            mongoFilter &= Builders<PropertyEntity>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(filter.Name, "i"));

        if (!string.IsNullOrEmpty(filter.Address))
            mongoFilter &= Builders<PropertyEntity>.Filter.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(filter.Address, "i"));

        if (filter.MinPrice.HasValue)
            mongoFilter &= Builders<PropertyEntity>.Filter.Gte(p => p.Price, filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            mongoFilter &= Builders<PropertyEntity>.Filter.Lte(p => p.Price, filter.MaxPrice.Value);

        if (filter.Year.HasValue)
            mongoFilter &= Builders<PropertyEntity>.Filter.Eq(p => p.Year, filter.Year.Value);

        if (!string.IsNullOrEmpty(filter.IdOwner))
            mongoFilter &= Builders<PropertyEntity>.Filter.Eq(p => p.IdOwner, filter.IdOwner);

        if (!string.IsNullOrEmpty(filter.CodeInternal))
            mongoFilter &= Builders<PropertyEntity>.Filter.Eq(p => p.CodeInternal, filter.CodeInternal);

        var pipeline = new[]
        {
            new BsonDocument("$match", mongoFilter.ToBsonDocument()),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Owners" },
                { "localField", "idOwner" },
                { "foreignField", "_id" },
                { "as", "owner" }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "PropertyImages" },
                { "localField", "_id" },
                { "foreignField", "idProperty" },
                { "as", "images" }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "PropertyTraces" },
                { "localField", "_id" },
                { "foreignField", "idProperty" },
                { "as", "propertyTraces" }
            })
        };

        var results = await _properties.Aggregate<PropertyEntity>(pipeline).ToListAsync();
        return results;
    }

    public async Task<PropertyEntity?> GetByCodeInternalAsync(string codeInternal)
    {
        return await _properties.Find(p => p.CodeInternal == codeInternal).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetPropertiesWithDetailsAsync()
    {
        return await GetAllWithDetailsAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetAllWithDetailsAsync()
    {
        var pipeline = new[]
        {
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Owners" },
                { "localField", "idOwner" },
                { "foreignField", "_id" },
                { "as", "owner" }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "PropertyImages" },
                { "localField", "_id" },
                { "foreignField", "idProperty" },
                { "as", "images" }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "PropertyTraces" },
                { "localField", "_id" },
                { "foreignField", "idProperty" },
                { "as", "propertyTraces" }
            }),
            new BsonDocument("$addFields", new BsonDocument
            {
                { "Owner", new BsonDocument("$arrayElemAt", new BsonArray { "$owner", 0 }) },
                { "Images", "$images" },
                { "PropertyTraces", "$propertyTraces" }
            }),
            new BsonDocument("$unset", new BsonArray { "owner", "images", "propertyTraces" })
        };

        return await _properties.Aggregate<PropertyEntity>(pipeline).ToListAsync();
    }

    public async Task<PropertyEntity?> GetWithDetailsAsync(string id)
    {
        return await GetByIdWithDetailsAsync(id);
    }

    public async Task<PropertyEntity?> GetByIdWithDetailsAsync(string id)
    {
        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument("_id", new MongoDB.Bson.ObjectId(id))),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Owners" },
                { "localField", "idOwner" },
                { "foreignField", "_id" },
                { "as", "owner" }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "PropertyImages" },
                { "localField", "_id" },
                { "foreignField", "idProperty" },
                { "as", "images" }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "PropertyTraces" },
                { "localField", "_id" },
                { "foreignField", "idProperty" },
                { "as", "propertyTraces" }
            }),
            new BsonDocument("$addFields", new BsonDocument
            {
                { "Owner", new BsonDocument("$arrayElemAt", new BsonArray { "$owner", 0 }) },
                { "Images", "$images" },
                { "PropertyTraces", "$propertyTraces" }
            }),
            new BsonDocument("$unset", new BsonArray { "owner", "images", "propertyTraces" })
        };

        return await _properties.Aggregate<PropertyEntity>(pipeline).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetPropertiesByYearRangeAsync(int minYear, int maxYear)
    {
        var filter = Builders<PropertyEntity>.Filter.And(
            Builders<PropertyEntity>.Filter.Gte(p => p.Year, minYear),
            Builders<PropertyEntity>.Filter.Lte(p => p.Year, maxYear)
        );
        return await _properties.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var filter = Builders<PropertyEntity>.Filter.And(
            Builders<PropertyEntity>.Filter.Gte(p => p.Price, minPrice),
            Builders<PropertyEntity>.Filter.Lte(p => p.Price, maxPrice)
        );
        return await _properties.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> SearchPropertiesAsync(string searchTerm)
    {
        var filter = Builders<PropertyEntity>.Filter.Or(
            Builders<PropertyEntity>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
            Builders<PropertyEntity>.Filter.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
            Builders<PropertyEntity>.Filter.Regex(p => p.CodeInternal, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
        );
        return await _properties.Find(filter).ToListAsync();
    }

    public async Task<bool> CodeInternalExistsAsync(string codeInternal, string? excludeId = null)
    {
        var filter = Builders<PropertyEntity>.Filter.Eq(p => p.CodeInternal, codeInternal);
        if (!string.IsNullOrEmpty(excludeId))
        {
            filter &= Builders<PropertyEntity>.Filter.Ne(p => p.Id, excludeId);
        }
        var count = await _properties.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task<decimal> GetAveragePriceAsync()
    {
        var pipeline = new[]
        {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "averagePrice", new BsonDocument("$avg", "$price") }
            })
        };

        var result = await _properties.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
        return result?["averagePrice"]?.AsDecimal ?? 0;
    }

    public async Task<decimal> GetAveragePriceByOwnerAsync(string ownerId)
    {
        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument("idOwner", new MongoDB.Bson.ObjectId(ownerId))),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "averagePrice", new BsonDocument("$avg", "$price") }
            })
        };

        var result = await _properties.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
        return result?["averagePrice"]?.AsDecimal ?? 0;
    }

    public async Task<IEnumerable<PropertyEntity>> GetVintagePropertiesAsync()
    {
        var currentYear = DateTime.Now.Year;
        var vintageYear = currentYear - 50;
        var filter = Builders<PropertyEntity>.Filter.Lt(p => p.Year, vintageYear);
        return await _properties.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetRecentPropertiesAsync(int days = 30)
    {
        var dateThreshold = DateTime.UtcNow.AddDays(-days);
        var filter = Builders<PropertyEntity>.Filter.Gte(p => p.CreatedAt, dateThreshold);
        return await _properties.Find(filter).ToListAsync();
    }

    #region IQueryableRepository<T> Implementation

    public async Task<IEnumerable<PropertyEntity>> FindAsync(Expression<Func<PropertyEntity, bool>> predicate)
    {
        return await _properties.Find(predicate).ToListAsync();
    }

    public async Task<PropertyEntity?> FindOneAsync(Expression<Func<PropertyEntity, bool>> predicate)
    {
        return await _properties.Find(predicate).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetPagedAsync(int skip, int take)
    {
        return await _properties.Find(_ => true).Skip(skip).Limit(take).ToListAsync();
    }

    public async Task<IEnumerable<PropertyEntity>> GetPagedAsync(Expression<Func<PropertyEntity, bool>> predicate, int skip, int take)
    {
        return await _properties.Find(predicate).Skip(skip).Limit(take).ToListAsync();
    }

    public async Task<long> CountAsync(Expression<Func<PropertyEntity, bool>> predicate)
    {
        return await _properties.CountDocumentsAsync(predicate);
    }

    public async Task<long> CountAsync()
    {
        return await _properties.CountDocumentsAsync(_ => true);
    }

    #endregion

    #region IAsyncQueryableRepository<T> Implementation

    public async Task<IEnumerable<TResult>> ProjectAsync<TResult>(Expression<Func<PropertyEntity, TResult>> selector)
    {
        return await _properties.Find(_ => true).Project(selector).ToListAsync();
    }

    public async Task<IEnumerable<TResult>> ProjectAsync<TResult>(
        Expression<Func<PropertyEntity, bool>> predicate,
        Expression<Func<PropertyEntity, TResult>> selector)
    {
        return await _properties.Find(predicate).Project(selector).ToListAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<PropertyEntity, bool>> predicate)
    {
        return await _properties.Find(predicate).AnyAsync();
    }

    public async Task<PropertyEntity?> FirstOrDefaultAsync(Expression<Func<PropertyEntity, bool>> predicate)
    {
        return await _properties.Find(predicate).FirstOrDefaultAsync();
    }

    public async Task<PropertyEntity> FirstAsync(Expression<Func<PropertyEntity, bool>> predicate)
    {
        return await _properties.Find(predicate).FirstAsync();
    }

    #endregion
}
