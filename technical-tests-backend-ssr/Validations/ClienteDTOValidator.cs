using FluentValidation;

public class ClienteDTOValidator : AbstractValidator<ClienteDTO>
{
    public ClienteDTOValidator()
    {
        RuleFor(c => c.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres.");

        RuleFor(c => c.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .Length(3, 100).WithMessage("El apellido debe tener entre 3 y 100 caracteres.");

        RuleFor(c => c.Telefono)
            .Length(6, 15).WithMessage("El teléfono debe tener entre 6 y 15 caracteres.");

        RuleFor(c => c.Email)
            .EmailAddress().WithMessage("El email no tiene un formato válido.");
    }
}
