using Plainly.Shared;
using Plainly.Shared.Interfaces;


namespace Plainly.Api.Exceptions;

public class BadRequestException : Exception, IHttpException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public BadRequestException() : base(Messages.BadRequest)
    { }

    public BadRequestException(string? message) : base(message ?? Messages.BadRequest)
    { }

    public BadRequestException(string? message, Exception? innerException) : base(message ?? Messages.BadRequest, innerException)
    { }
}
