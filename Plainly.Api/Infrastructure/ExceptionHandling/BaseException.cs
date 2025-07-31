using Plainly.Shared.Responses;


namespace Plainly.Api.Infrastructure.ExceptionHandling;

public abstract class BaseException : Exception
{
    protected BaseException(string? message) : base(message)
    {
    }

    protected BaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public abstract ErrorResponse ToResponse(string traceId);
}
