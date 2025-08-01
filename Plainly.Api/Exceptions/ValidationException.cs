using Plainly.Shared;
using Plainly.Shared.Interfaces;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class ValidationException : Exception, IHttpException, IExceptionWithErrors
{
    public int StatusCode => StatusCodes.Status422UnprocessableEntity;

    public ErrorDetail[]? Errors { get; } = null;


    public ValidationException() : base(Messages.ValidationError)
    {
    }

    public ValidationException(ErrorDetail[] errors) : base(Messages.ValidationError)
    {
        Errors = errors;
    }

    public ValidationException(string? message) : base(message ?? Messages.ValidationError)
    {
    }

    public ValidationException(string? message, ErrorDetail[] errors) : base(message ?? Messages.ValidationError)
    {
        Errors = errors;
    }

    public ValidationException(string? message, Exception? innerException) : base(message ?? Messages.ValidationError, innerException)
    {
    }

    public ValidationException(string? message, ErrorDetail[] errors, Exception? innerException) : base(message ?? Messages.ValidationError, innerException)
    {
        Errors = errors;
    }
}
