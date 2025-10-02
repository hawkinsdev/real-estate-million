using RealEstate.Application.Modules.Property.DTOs;

namespace RealEstate.Application.Modules.Owner.DTOs;

public class OwnerWithPropertiesDto
{
    public string IdOwner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Age { get; set; }
    public ICollection<PropertyDto> Properties { get; set; } = [];
}
