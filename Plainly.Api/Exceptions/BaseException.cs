using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public abstract class BaseException : Exception
{
    public abstract int StatusCode { get; }

    protected BaseException() : base()
    {
    }

    protected BaseException(string? message) : base(message)
    {
    }

    protected BaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public abstract ErrorResponse ToResponse();
}
