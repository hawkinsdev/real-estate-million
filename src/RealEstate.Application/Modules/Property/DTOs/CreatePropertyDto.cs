using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Modules.Property.DTOs;

public class CreatePropertyDto
{
    [Required(ErrorMessage = "El nombre de la propiedad es requerido")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es requerida")]
    [StringLength(500, ErrorMessage = "La dirección no puede exceder 500 caracteres")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "El código interno es requerido")]
    [StringLength(50, ErrorMessage = "El código interno no puede exceder 50 caracteres")]
    public string CodeInternal { get; set; } = string.Empty;

    [Required(ErrorMessage = "El año es requerido")]
    [Range(1800, 2100, ErrorMessage = "El año debe estar entre 1800 y 2100")]
    public int Year { get; set; }

    [Required(ErrorMessage = "El ID del propietario es requerido")]
    public string IdOwner { get; set; } = string.Empty;
}
