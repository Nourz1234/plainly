using Plainly.Api.Exceptions;
using Plainly.Api.Extensions;
using Plainly.Domain;
using Plainly.Domain.Exceptions;
using Plainly.Domain.Extensions;
using Plainly.Shared.Extensions;
using Plainly.Shared.Responses;

namespace Plainly.Api.Builders;

public class ErrorResponseBuilder
{
    private static int StatusCodeForDomainErrorType(DomainErrorType errorType) => errorType switch
    {
        DomainErrorType.InternalError => StatusCodes.Status500InternalServerError,
        DomainErrorType.InvalidOperation => StatusCodes.Status400BadRequest,
        DomainErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        DomainErrorType.Forbidden => StatusCodes.Status403Forbidden,
        DomainErrorType.NotFound => StatusCodes.Status404NotFound,
        DomainErrorType.Conflict => StatusCodes.Status409Conflict,
        DomainErrorType.ValidationError => StatusCodes.Status422UnprocessableEntity,
        DomainErrorType.PreconditionFailed => StatusCodes.Status412PreconditionFailed,
        DomainErrorType.ResourceLocked => StatusCodes.Status423Locked,
        DomainErrorType.DependencyFailure => StatusCodes.Status424FailedDependency,
        DomainErrorType.RateLimitExceeded => StatusCodes.Status429TooManyRequests,
        DomainErrorType.NotImplemented => StatusCodes.Status501NotImplemented,
        _ => throw new ArgumentOutOfRangeException(nameof(errorType), errorType, null),
    };

    public static ErrorResponseBuilder FromErrorCode(DomainErrorCode errorCode)
    {
        var errorType = errorCode.GetErrorType();
        var statusCode = StatusCodeForDomainErrorType(errorType);
        return new ErrorResponseBuilder(statusCode, errorCode.ToString(), errorCode.GetDescription());
    }

    public static ErrorResponseBuilder FromDomainError(DomainException error)
    {
        var statusCode = StatusCodeForDomainErrorType(error.Type);
        var builder = new ErrorResponseBuilder(statusCode, error.ErrorCode, error.Description);
        if (error.Errors != null)
        {
            builder.WithErrors(error.Errors);
        }
        return builder;
    }

    public static ErrorResponseBuilder FromApiException(ApiException exception)
        => new(exception.StatusCode, exception.ErrorCode, exception.Message);

    protected readonly bool _Success = false;
    protected readonly int _StatusCode;
    protected readonly string _ErrorCode;
    protected readonly string _Message;
    protected ErrorDetail[]? _Errors;

    private ErrorResponseBuilder(int statusCode, string errorCode, string message)
    {
        _StatusCode = statusCode;
        _ErrorCode = errorCode;
        _Message = message;
    }


    public ErrorResponseBuilder WithErrors(ErrorDetail[] errors)
    {
        _Errors = errors;
        return this;
    }

    public ErrorResponse Build(HttpContext context) => new()
    {
        Success = _Success,
        StatusCode = _StatusCode,
        ErrorCode = _ErrorCode,
        Message = _Message,
        Errors = _Errors,
        TraceId = _StatusCode is < 500 ? null : context.GetTraceId()
    };
}