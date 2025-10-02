using FluentValidation;

namespace RealEstate.Application.Modules.Owner.Validators
{
    public class CreateOwnerValidator : AbstractValidator<CreateOwnerDto>
    {
        public CreateOwnerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(200).WithMessage("La dirección no puede superar los 200 caracteres.");

            RuleFor(x => x.Birthday)
                .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria.")
                .LessThan(DateTime.UtcNow).WithMessage("La fecha de nacimiento debe ser en el pasado.");

            RuleFor(x => x.Photo)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Photo))
                .WithMessage("La foto debe ser una URL válida.");
        }
    }
}
