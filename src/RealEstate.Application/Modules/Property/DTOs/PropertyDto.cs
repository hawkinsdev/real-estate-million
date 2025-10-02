using RealEstate.Application.Modules.Owner.DTOs;

namespace RealEstate.Application.Modules.Property.DTOs;

public class PropertyDto
{
    public string IdProperty { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public string IdOwner { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public OwnerDto? Owner { get; set; }
    public ICollection<PropertyImageDto> Images { get; set; } = [];
    public ICollection<PropertyTraceDto> PropertyTraces { get; set; } = [];
}