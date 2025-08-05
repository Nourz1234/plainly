using System.ComponentModel;

namespace Plainly.Domain;

/// <summary>
/// All errors should be defined here
/// </summary>
public enum ErrorCode
{
    [Category(ErrorCategories.InternalError)]
    [Description(Messages.InternalError)]
    InternalError,

    [Category(ErrorCategories.BadRequest)]
    [Description(Messages.BadRequest)]
    BadRequest,

    [Category(ErrorCategories.Unauthorized)]
    [Description(Messages.Unauthorized)]
    Unauthorized,

    [Category(ErrorCategories.Forbidden)]
    [Description(Messages.Forbidden)]
    Forbidden,

    [Category(ErrorCategories.NotFound)]
    [Description(Messages.NotFound)]
    NotFound,

    [Category(ErrorCategories.ValidationError)]
    [Description(Messages.ValidationError)]
    ValidationError,

    [Category(ErrorCategories.Unauthorized)]
    [Description(Messages.InvalidLoginCredentials)]
    InvalidLoginCredentials,

    [Category(ErrorCategories.Forbidden)]
    [Description(Messages.EmailNotConfirmed)]
    EmailNotConfirmed,

    [Category(ErrorCategories.ResourceLocked)]
    [Description(Messages.UserIsLockedOut)]
    UserIsLockedOut,

    [Category(ErrorCategories.Forbidden)]
    [Description(Messages.UserIsNotActive)]
    UserIsNotActive,
}