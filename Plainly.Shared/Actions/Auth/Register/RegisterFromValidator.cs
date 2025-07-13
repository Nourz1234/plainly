using FluentValidation;

namespace Plainly.Shared.Actions.Auth.Register;


public class RegisterFormValidator : AbstractValidator<RegisterForm>
{
    public RegisterFormValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithErrorCode(ErrorCode.FullNameRequired.ToString());
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode(ErrorCode.EmailRequired.ToString())
            .EmailAddress().WithErrorCode(ErrorCode.InvalidEmail.ToString());
        RuleFor(x => x.Password)
               .NotEmpty().WithErrorCode(ErrorCode.PasswordRequired.ToString())
               .MinimumLength(8).WithErrorCode(ErrorCode.PasswordTooShort.ToString())
               .Matches("[A-Z]").WithMessage(PasswordMissingUppercase).WithErrorCode(ErrorCode.PasswordMissingUppercase.ToString())
               .Matches("[a-z]").WithMessage(PasswordMissingLowercase).WithErrorCode(ErrorCode.PasswordMissingLowercase.ToString())
               .Matches("[0-9]").WithMessage(PasswordMissingDigit).WithErrorCode(ErrorCode.PasswordMissingDigit.ToString())
               .Matches("[^a-zA-Z0-9]").WithMessage(PasswordMissingSpecial).WithErrorCode(ErrorCode.PasswordMissingSpecial.ToString());
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithErrorCode(ErrorCode.PasswordMismatch.ToString());
    }

    public const string PasswordMissingUppercase = "'{PropertyName}' must contain at least one uppercase letter.";
    public const string PasswordMissingLowercase = "'{PropertyName}' must contain at least one lowercase letter.";
    public const string PasswordMissingDigit = "'{PropertyName}' must contain at least one digit.";
    public const string PasswordMissingSpecial = "'{PropertyName}' must contain at least one special character.";
}