namespace RealEstate.Application.Modules.Property.DTOs;

public class PropertyTraceFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public string? IdProperty { get; set; }
}