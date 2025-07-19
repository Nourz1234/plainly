using FluentValidation;

namespace Plainly.Shared.Actions.Auth.Login;

public class LoginFormValidator : AbstractValidator<LoginForm>
{
    public LoginFormValidator()
    {
        RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ErrorCode.EmailRequired.ToString())
            .EmailAddress().WithErrorCode(ErrorCode.InvalidEmail.ToString());
        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode(ErrorCode.PasswordRequired.ToString());
    }
}
