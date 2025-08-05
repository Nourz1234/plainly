using System.ComponentModel;

namespace Plainly.Shared;

public enum ValidationError
{
    [Description("Field is required.")]
    FullNameRequired,

    [Description("Field is required.")]
    EmailRequired,

    [Description("Invalid email address.")]
    InvalidEmail,

    [Description("Field is required.")]
    PasswordRequired,

    [Description("Password must be at least {MinLength} characters long.")]
    PasswordTooShort,

    [Description("Password must contain at least one uppercase letter.")]
    PasswordMissingUppercase,

    [Description("Password must contain at least one lowercase letter.")]
    PasswordMissingLowercase,

    [Description("Password must contain at least one digit.")]
    PasswordMissingDigit,

    [Description("Password must contain at least one special character.")]
    PasswordMissingSpecial,

    [Description("Passwords do not match.")]
    PasswordMismatch,

    [Description("Unknown field.")]
    UnknownField,
}