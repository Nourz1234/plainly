using FluentValidation;
using Plainly.Shared.Extensions;

namespace Plainly.Shared.Actions.Auth.Login;

public class LoginFormValidator : AbstractValidator<LoginForm>
{
    public LoginFormValidator()
    {
        RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
            .NotEmpty().WithValidationError(ValidationError.EmailRequired)
            .EmailAddress().WithValidationError(ValidationError.InvalidEmail);
        RuleFor(x => x.Password)
            .NotEmpty().WithValidationError(ValidationError.PasswordRequired);
    }
}
