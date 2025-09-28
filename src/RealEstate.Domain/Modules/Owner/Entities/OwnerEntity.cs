using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RealEstate.Domain.Common.Entities;
using RealEstate.Domain.Common.Attributes;
using RealEstate.Domain.Modules.Property.Entities;

namespace RealEstate.Domain.Modules.Owner.Entities
{
    [BsonCollection("Owners")]
    public class OwnerEntity : BaseEntity<string>
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("photo")]
        public string Photo { get; set; } = string.Empty;

        [BsonElement("birthday")]
        public DateTime Birthday { get; set; }

        // Navigation properties (populated via aggregation)
        [BsonIgnore]
        public ICollection<PropertyEntity> Properties { get; set; } = [];

        // Business methods
        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - Birthday.Year;
            if (Birthday.Date > today.AddYears(-age)) age--;
            return age;
        }

        public bool IsAdult()
        {
            return GetAge() >= 18;
        }

        public void UpdatePhoto(string photoUrl)
        {
            if (!string.IsNullOrWhiteSpace(photoUrl))
            {
                Photo = photoUrl;
                UpdateTimestamp();
            }
        }
    }
}