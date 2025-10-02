using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RealEstate.Domain.Modules.Owner.Entities;
using RealEstate.Domain.Modules.Property.Entities;
using RealEstate.Infrastructure.Common.Configuration;

namespace RealEstate.Infrastructure.Common.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var mongoSettings = settings.Value;
            var client = new MongoClient(mongoSettings.ConnectionString);
            _database = client.GetDatabase(mongoSettings.DatabaseName);
        }

        public IMongoCollection<PropertyEntity> Properties =>
            _database.GetCollection<PropertyEntity>("Properties");

        public IMongoCollection<OwnerEntity> Owners =>
            _database.GetCollection<OwnerEntity>("Owners");

        public IMongoDatabase Database => _database;
    }
}