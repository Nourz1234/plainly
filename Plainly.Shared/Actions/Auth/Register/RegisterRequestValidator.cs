using FluentValidation;

namespace Plainly.Shared.Auth.Register;


public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    RegisterRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
    }
}