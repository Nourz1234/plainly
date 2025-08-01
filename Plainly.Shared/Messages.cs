namespace Plainly.Shared;

public static class Messages
{
    // General
    public const string Success = "Operation completed successfully!";
    public const string InternalServerError = "Something went wrong. Please try again later.";
    public const string NotFound = "The requested resource could not be found.";
    public const string Forbidden = "You do not have permission to perform this action.";
    public const string BadRequest = "The request was invalid.";
    public const string Unauthorized = "You are not authorized to perform this action.";
    // Specific
    public const string EndpointNotFound = "The requested endpoint could not be found.";
    public const string InvalidLoginCredentials = "Invalid email or password.";
    public const string ValidationError = "One or more validation errors occurred.";
    public const string UserIsNotActive = "User is not active. Please contact your administrator.";
    public const string UnknownField = "'{0}' is not a recognized field.";
    public const string UserIsLockedOut = "User is locked out due to too many failed login attempts.";
    public const string EmailNotConfirmed = "Please confirm your email address.";
}