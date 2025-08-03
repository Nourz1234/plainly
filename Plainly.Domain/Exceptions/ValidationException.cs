namespace Plainly.Domain.Exceptions;

public class ValidationException : BaseException
{
    // public ErrorDetail[] Errors { get; }

    public ValidationException() : base()
    {
    }

    public ValidationException(string? message) : base(message)
    {
    }

    public ValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}