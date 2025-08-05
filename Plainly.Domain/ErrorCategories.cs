namespace Plainly.Domain;


public static class ErrorCategories
{
    public const string InternalError = "InternalError";
    public const string BadRequest = "BadRequest";
    public const string Unauthorized = "Unauthorized";
    public const string Forbidden = "Forbidden";
    public const string NotFound = "NotFound";
    public const string Conflict = "Conflict";
    public const string ValidationError = "ValidationError";
    public const string BusinessRuleViolation = "BusinessRuleViolation";
    public const string PreconditionFailed = "PreconditionFailed";
    public const string InvariantViolation = "InvariantViolation";
    public const string DependencyFailure = "DependencyFailure";
    public const string InvalidOperation = "InvalidOperation";
    public const string ResourceLocked = "ResourceLocked";
}