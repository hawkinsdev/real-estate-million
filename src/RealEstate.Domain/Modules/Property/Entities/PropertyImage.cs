
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RealEstate.Domain.Common.Entities;
using RealEstate.Domain.Common.Attributes;

namespace RealEstate.Domain.Modules.Property.Entities
{
    [BsonCollection("PropertyImages")]
    public class PropertyImage : BaseEntity<string>
    {
        [BsonElement("idProperty")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdProperty { get; set; } = string.Empty;

        [BsonElement("file")]
        public string File { get; set; } = string.Empty;

        [BsonElement("enabled")]
        public bool Enabled { get; set; } = true;

        // Navigation property
        [BsonIgnore]
        public PropertyEntity? Property { get; set; }

        // Business methods
        public void Enable()
        {
            Enabled = true;
            UpdateTimestamp();
        }

        public void Disable()
        {
            Enabled = false;
            UpdateTimestamp();
        }

        public bool IsValidImage()
        {
            if (string.IsNullOrWhiteSpace(File)) return false;

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            return validExtensions.Any(ext => File.ToLower().EndsWith(ext));
        }
    }
}