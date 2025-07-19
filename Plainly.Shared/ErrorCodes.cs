namespace Plainly.Shared;

public enum ErrorCode
{
    FullNameRequired,
    EmailRequired,
    InvalidEmail,
    PasswordRequired,
    PasswordTooShort,
    PasswordMissingUppercase,
    PasswordMissingLowercase,
    PasswordMissingDigit,
    PasswordMissingSpecial,
    PasswordMismatch,
    UnknownField,
}