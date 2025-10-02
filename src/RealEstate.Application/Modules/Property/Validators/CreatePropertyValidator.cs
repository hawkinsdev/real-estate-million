using FluentValidation;
using RealEstate.Application.Modules.Property.DTOs;

namespace RealEstate.Application.Modules.Property.Validators;

public class CreatePropertyValidator : AbstractValidator<CreatePropertyDto>
{
    public CreatePropertyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la propiedad es requerido")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("La dirección es requerida")
            .MaximumLength(500).WithMessage("La dirección no puede exceder 500 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

        RuleFor(x => x.CodeInternal)
            .NotEmpty().WithMessage("El código interno es requerido")
            .MaximumLength(50).WithMessage("El código interno no puede exceder 50 caracteres");

        RuleFor(x => x.Year)
            .InclusiveBetween(1800, 2100).WithMessage("El año debe estar entre 1800 y 2100");

        RuleFor(x => x.IdOwner)
            .NotEmpty().WithMessage("El ID del propietario es requerido");
    }
}
