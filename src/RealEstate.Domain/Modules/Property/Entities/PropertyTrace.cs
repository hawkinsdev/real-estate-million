using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RealEstate.Domain.Common.Entities;
using RealEstate.Domain.Common.Attributes;

namespace RealEstate.Domain.Modules.Property.Entities
{
    [BsonCollection("PropertyTraces")]
    public class PropertyTrace : BaseEntity<string>
    {
        [BsonElement("dateSale")]
        public DateTime DateSale { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("value")]
        public decimal Value { get; set; }

        [BsonElement("tax")]
        public decimal Tax { get; set; }

        [BsonElement("idProperty")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdProperty { get; set; } = string.Empty;

        // Navigation property
        [BsonIgnore]
        public PropertyEntity? Property { get; set; }

        // Business methods
        public decimal GetTotalCost()
        {
            return Value + Tax;
        }

        public decimal GetTaxPercentage()
        {
            return Value > 0 ? (Tax / Value) * 100 : 0;
        }

        public bool IsFutureSale()
        {
            return DateSale > DateTime.Now;
        }

        public bool IsRecentSale(int daysThreshold = 30)
        {
            return (DateTime.Now - DateSale).TotalDays <= daysThreshold;
        }
    }
}