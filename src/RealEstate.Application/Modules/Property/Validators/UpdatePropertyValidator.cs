using FluentValidation;
using RealEstate.Application.Modules.Property.DTOs;

namespace RealEstate.Application.Modules.Property.Validators;

public class UpdatePropertyValidator : AbstractValidator<UpdatePropertyDto>
{
    public UpdatePropertyValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("La dirección no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0")
            .When(x => x.Price.HasValue);

        RuleFor(x => x.CodeInternal)
            .MaximumLength(50).WithMessage("El código interno no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.CodeInternal));

        RuleFor(x => x.Year)
            .InclusiveBetween(1800, 2100).WithMessage("El año debe estar entre 1800 y 2100")
            .When(x => x.Year.HasValue);
    }
}
