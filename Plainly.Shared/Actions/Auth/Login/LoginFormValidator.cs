using FluentValidation;

namespace Plainly.Shared.Actions.Auth.Login;

public class LoginFormValidator : AbstractValidator<LoginForm>
{
    public LoginFormValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
