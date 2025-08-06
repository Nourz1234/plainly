namespace Plainly.Domain;

public enum DomainErrorType
{
    InternalError,
    Unauthorized,
    Forbidden,
    NotFound,
    Conflict,
    ValidationError,
    PreconditionFailed,
    DependencyFailure,
    InvalidOperation,
    ResourceLocked,
    RateLimitExceeded,
    NotImplemented,
}