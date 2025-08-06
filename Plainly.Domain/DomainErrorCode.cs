using System.ComponentModel;
using Plainly.Domain.Attributes;

namespace Plainly.Domain;

/// <summary>
/// All errors should be defined here
/// </summary>
public enum DomainErrorCode
{
    [ErrorType(DomainErrorType.InternalError)]
    [Description(DomainMessages.InternalError)]
    InternalError,

    [ErrorType(DomainErrorType.InvalidOperation)]
    [Description(DomainMessages.InvalidOperation)]
    InvalidOperation,

    [ErrorType(DomainErrorType.Unauthorized)]
    [Description(DomainMessages.Unauthorized)]
    Unauthorized,

    [ErrorType(DomainErrorType.Forbidden)]
    [Description(DomainMessages.Forbidden)]
    Forbidden,

    [ErrorType(DomainErrorType.NotFound)]
    [Description(DomainMessages.NotFound)]
    NotFound,

    [ErrorType(DomainErrorType.ValidationError)]
    [Description(DomainMessages.ValidationError)]
    ValidationError,

    [ErrorType(DomainErrorType.NotImplemented)]
    [Description(DomainMessages.NotImplemented)]
    NotImplemented,

    [ErrorType(DomainErrorType.Unauthorized)]
    [Description(DomainMessages.InvalidLoginCredentials)]
    InvalidLoginCredentials,

    [ErrorType(DomainErrorType.Forbidden)]
    [Description(DomainMessages.EmailNotConfirmed)]
    EmailNotConfirmed,

    [ErrorType(DomainErrorType.ResourceLocked)]
    [Description(DomainMessages.UserIsLockedOut)]
    UserIsLockedOut,

    [ErrorType(DomainErrorType.Forbidden)]
    [Description(DomainMessages.UserIsNotActive)]
    UserIsNotActive,
}