using FluentValidation;
using Plainly.Shared.Extensions;

namespace Plainly.Shared.Actions.Auth.Register;

public class RegisterFormValidator : AbstractValidator<RegisterForm>
{
    public RegisterFormValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithValidationError(ValidationError.FullNameRequired);
        RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
            .NotEmpty().WithValidationError(ValidationError.EmailRequired)
            .EmailAddress().WithValidationError(ValidationError.InvalidEmail);
        RuleFor(x => x.Password)
            .NotEmpty().WithValidationError(ValidationError.PasswordRequired).DependentRules(() =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(8).WithValidationError(ValidationError.PasswordTooShort)
                        .Matches("[A-Z]").WithValidationError(ValidationError.PasswordMissingUppercase)
                        .Matches("[a-z]").WithValidationError(ValidationError.PasswordMissingLowercase)
                        .Matches("[0-9]").WithValidationError(ValidationError.PasswordMissingDigit)
                        .Matches("[^a-zA-Z0-9]").WithValidationError(ValidationError.PasswordMissingSpecial);
            });
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithValidationError(ValidationError.PasswordMismatch);
    }
}