using System.ComponentModel;

namespace Plainly.Shared;

public enum ErrorCode
{
    // TODO: give each code a description and use error codes in response
    // also an extension method to convert to ErrorDetail
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