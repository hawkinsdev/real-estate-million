using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Modules.Property.DTOs;

public class UpdatePropertyDto
{
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string? Name { get; set; }

    [StringLength(500, ErrorMessage = "La dirección no puede exceder 500 caracteres")]
    public string? Address { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal? Price { get; set; }

    [StringLength(50, ErrorMessage = "El código interno no puede exceder 50 caracteres")]
    public string? CodeInternal { get; set; }

    [Range(1800, 2100, ErrorMessage = "El año debe estar entre 1800 y 2100")]
    public int? Year { get; set; }

    public string? IdOwner { get; set; }
}
