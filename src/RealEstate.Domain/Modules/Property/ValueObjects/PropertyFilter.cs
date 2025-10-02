namespace RealEstate.Domain.Modules.Property.ValueObjects;

public class PropertyFilter
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? Year { get; set; }
    public string? IdOwner { get; set; }
    public string? CodeInternal { get; set; }
}
