using MongoDB.Driver;
using System.Linq.Expressions;
using RealEstate.Domain.Modules.Owner.Entities;
using RealEstate.Domain.Modules.Owner.Interfaces;
using RealEstate.Domain.Modules.Property.Entities;
using RealEstate.Infrastructure.Common.Data;

namespace RealEstate.Infrastructure.Modules.Owner.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IMongoCollection<OwnerEntity> _owners;
        private readonly IMongoCollection<PropertyEntity> _properties;

        public OwnerRepository(MongoDbContext context)
        {
            _owners = context.Database.GetCollection<OwnerEntity>("owners");
            _properties = context.Database.GetCollection<PropertyEntity>("properties");
        }

        #region IRepository<T>

        public async Task<IEnumerable<OwnerEntity>> GetAllAsync()
        {
            return await _owners.Find(_ => true).ToListAsync();
        }

        public async Task<OwnerEntity?> GetByIdAsync(string id)
        {
            return await _owners.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<OwnerEntity> CreateAsync(OwnerEntity entity)
        {
            await _owners.InsertOneAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(string id, OwnerEntity entity)
        {
            await _owners.ReplaceOneAsync(x => x.Id == id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _owners.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _owners.Find(x => x.Id == id).AnyAsync();
        }

        public async Task<long> CountAsync()
        {
            return await _owners.CountDocumentsAsync(_ => true);
        }

        #endregion

        #region IQueryableRepository<T>

        public async Task<IEnumerable<OwnerEntity>> FindAsync(Expression<Func<OwnerEntity, bool>> predicate)
        {
            return await _owners.Find(predicate).ToListAsync();
        }

        public async Task<OwnerEntity?> FindOneAsync(Expression<Func<OwnerEntity, bool>> predicate)
        {
            return await _owners.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetPagedAsync(int skip, int take)
        {
            return await _owners.Find(_ => true).Skip(skip).Limit(take).ToListAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetPagedAsync(Expression<Func<OwnerEntity, bool>> predicate, int skip, int take)
        {
            return await _owners.Find(predicate).Skip(skip).Limit(take).ToListAsync();
        }

        public async Task<long> CountAsync(Expression<Func<OwnerEntity, bool>> predicate)
        {
            return await _owners.CountDocumentsAsync(predicate);
        }

        #endregion

        #region IAsyncQueryableRepository<T>

        public async Task<IEnumerable<TResult>> ProjectAsync<TResult>(Expression<Func<OwnerEntity, TResult>> selector)
        {
            return await _owners.Find(_ => true).Project(selector).ToListAsync();
        }

        public async Task<IEnumerable<TResult>> ProjectAsync<TResult>(
            Expression<Func<OwnerEntity, bool>> predicate,
            Expression<Func<OwnerEntity, TResult>> selector)
        {
            return await _owners.Find(predicate).Project(selector).ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<OwnerEntity, bool>> predicate)
        {
            return await _owners.Find(predicate).AnyAsync();
        }

        public async Task<OwnerEntity?> FirstOrDefaultAsync(Expression<Func<OwnerEntity, bool>> predicate)
        {
            return await _owners.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<OwnerEntity> FirstAsync(Expression<Func<OwnerEntity, bool>> predicate)
        {
            return await _owners.Find(predicate).FirstAsync();
        }

        #endregion

        #region IOwnerRepository custom methods

        public async Task<IEnumerable<OwnerEntity>> GetOwnersByAgeRangeAsync(int minAge, int maxAge)
        {
            var today = DateTime.UtcNow;
            var minDate = today.AddYears(-maxAge - 1).AddDays(1);
            var maxDate = today.AddYears(-minAge);

            var filter = Builders<OwnerEntity>.Filter.And(
                Builders<OwnerEntity>.Filter.Gte(o => o.Birthday, minDate),
                Builders<OwnerEntity>.Filter.Lte(o => o.Birthday, maxDate)
            );

            return await _owners.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> SearchOwnersByNameAsync(string searchTerm)
        {
            var filter = Builders<OwnerEntity>.Filter.Regex(o => o.Name, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"));
            return await _owners.Find(filter).ToListAsync();
        }

        public async Task<OwnerEntity?> GetOwnerWithPropertiesAsync(string id)
        {
            var owner = await GetByIdAsync(id);
            if (owner == null) return null;

            var props = await _properties.Find(p => p.IdOwner == id).ToListAsync();
            owner.Properties = props;
            return owner;
        }

        public async Task<IEnumerable<OwnerEntity>> GetOwnersWithPropertiesAsync()
        {
            var owners = await GetAllAsync();

            foreach (var owner in owners)
            {
                var props = await _properties.Find(p => p.IdOwner == owner.Id).ToListAsync();
                owner.Properties = props;
            }

            return owners;
        }

        public async Task<bool> HasPropertiesAsync(string ownerId)
        {
            var count = await _properties.CountDocumentsAsync(p => p.IdOwner == ownerId);
            return count > 0;
        }

        public async Task<int> GetPropertyCountAsync(string ownerId)
        {
            var count = await _properties.CountDocumentsAsync(p => p.IdOwner == ownerId);
            return (int)count;
        }

        public async Task<IEnumerable<OwnerEntity>> GetOwnersByBirthdayMonthAsync(int month)
        {
            var filter = Builders<OwnerEntity>.Filter.Where(o => o.Birthday.Month == month);
            return await _owners.Find(filter).ToListAsync();
        }

        #endregion
    }
}
