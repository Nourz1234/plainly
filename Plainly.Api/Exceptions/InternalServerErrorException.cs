using Plainly.Shared;
using Plainly.Shared.Interfaces;


namespace Plainly.Api.Exceptions;

public class InternalServerErrorException : Exception, IHttpException
{
    public int StatusCode => StatusCodes.Status500InternalServerError;

    public InternalServerErrorException() : base(Messages.InternalServerError)
    { }

    public InternalServerErrorException(string? message) : base(message ?? Messages.InternalServerError)
    { }

    public InternalServerErrorException(string? message, Exception? innerException) : base(message ?? Messages.InternalServerError, innerException)
    { }
}
