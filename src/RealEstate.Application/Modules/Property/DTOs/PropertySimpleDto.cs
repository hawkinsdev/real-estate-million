namespace RealEstate.Application.Modules.Property.DTOs;

/// <summary>
/// DTO simplificado para la prueba técnica con los campos específicos requeridos
/// </summary>
public class PropertySimpleDto
{
    public string IdOwner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = string.Empty; // URL de la imagen principal
}
