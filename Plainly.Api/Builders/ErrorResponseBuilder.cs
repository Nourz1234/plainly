using Plainly.Api.Exceptions;
using Plainly.Api.Extensions;
using Plainly.Domain;
using Plainly.Shared.Extensions;
using Plainly.Shared.Responses;

namespace Plainly.Api.Builders;

public class ErrorResponseBuilder
{
    private static int StatusCodeForCategory(string category) => category switch
    {
        // General
        ErrorCategories.InternalError => StatusCodes.Status500InternalServerError,
        ErrorCategories.BadRequest => StatusCodes.Status400BadRequest,
        ErrorCategories.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorCategories.Forbidden => StatusCodes.Status403Forbidden,
        ErrorCategories.NotFound => StatusCodes.Status404NotFound,
        ErrorCategories.Conflict => StatusCodes.Status409Conflict,
        ErrorCategories.ValidationError => StatusCodes.Status422UnprocessableEntity,
        // Business
        ErrorCategories.BusinessRuleViolation => StatusCodes.Status409Conflict,
        ErrorCategories.InvariantViolation => StatusCodes.Status500InternalServerError,
        ErrorCategories.DependencyFailure => StatusCodes.Status503ServiceUnavailable,
        ErrorCategories.PreconditionFailed => StatusCodes.Status412PreconditionFailed,
        ErrorCategories.ResourceLocked => StatusCodes.Status423Locked,
        _ => StatusCodes.Status500InternalServerError,
    };

    public static ErrorResponseBuilder FromErrorCode(ErrorCode errorCode)
    {
        var category = errorCode.GetCategory();
        var statusCode = StatusCodeForCategory(category);
        return new ErrorResponseBuilder(statusCode, errorCode.ToString(), errorCode.GetDescription());
    }

    public static ErrorResponseBuilder FromDomainError(DomainError error)
    {
        var statusCode = StatusCodeForCategory(error.Category);
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