using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Modules.Owner;

public class UpdateOwnerDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
    public string Address { get; set; } = string.Empty;

    [Url(ErrorMessage = "Photo must be a valid URL")]
    public string Photo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birthday is required")]
    public DateTime Birthday { get; set; }
}