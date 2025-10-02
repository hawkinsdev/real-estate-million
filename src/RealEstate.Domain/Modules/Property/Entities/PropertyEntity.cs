using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RealEstate.Domain.Common.Entities;
using RealEstate.Domain.Common.Attributes;
using RealEstate.Domain.Modules.Owner.Entities;

namespace RealEstate.Domain.Modules.Property.Entities
{
    [BsonCollection("Properties")]
    public class PropertyEntity : BaseEntity<string>
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("codeInternal")]
        public string CodeInternal { get; set; } = string.Empty;

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("idOwner")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOwner { get; set; } = string.Empty;

        // Navigation properties (populated via aggregation)
        [BsonIgnore]
        public OwnerEntity? Owner { get; set; }

        [BsonIgnore]
        public ICollection<PropertyImage> Images { get; set; } = [];

        [BsonIgnore]
        public ICollection<PropertyTrace> PropertyTraces { get; set; } = [];

        // Business methods
        public int GetPropertyAge()
        {
            return DateTime.Now.Year - Year;
        }

        public bool IsVintage()
        {
            return GetPropertyAge() > 50;
        }

        public decimal GetTotalValue()
        {
            var latestTrace = PropertyTraces?.OrderByDescending(t => t.DateSale).FirstOrDefault();
            return latestTrace?.Value ?? Price;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice > 0)
            {
                Price = newPrice;
                UpdateTimestamp();
            }
        }

        public PropertyImage? GetMainImage()
        {
            return Images?.FirstOrDefault(i => i.Enabled);
        }

        public IEnumerable<PropertyImage> GetEnabledImages()
        {
            return Images?.Where(i => i.Enabled) ?? Enumerable.Empty<PropertyImage>();
        }
    }
}
