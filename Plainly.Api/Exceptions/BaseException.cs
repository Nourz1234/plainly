using Microsoft.AspNetCore.Mvc;


namespace Plainly.Api.Exceptions;

public abstract class BaseException : Exception
{
    public abstract int StatusCode { get; }
    public abstract string DefaultMessage { get; }

    protected BaseException() : base()
    {
    }

    protected BaseException(string? message) : base(message)
    {
    }

    protected BaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected string MessageOrDefault() => Message ?? DefaultMessage;

    public virtual object GetResult() => new
    {
        StatusCode,
        Status = "error",
        Message = MessageOrDefault(),
    };
}
