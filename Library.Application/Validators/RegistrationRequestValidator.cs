using FluentValidation;
using Library.Application.DTOs.Identity;

namespace Library.Application.Validators
{
    public class RegistrationRequestValidator:AbstractValidator<RegistrationRequestDTO>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(r => r.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits and can include an optional '+' at the beginning.");

            RuleFor(r => r.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => new[] { "ADMIN", "CLIENT" }.Contains(role))
            .WithMessage("Role must be one of the following: ADMIN, CLIENT.");
        }
    }
}
