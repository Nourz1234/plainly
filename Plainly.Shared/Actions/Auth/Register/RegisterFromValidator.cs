using FluentValidation;

namespace Plainly.Shared.Actions.Auth.Register;


public class RegisterFormValidator : AbstractValidator<RegisterForm>
{
    RegisterFormValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
    }
}