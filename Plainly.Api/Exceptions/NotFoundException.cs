using Plainly.Shared;
using Plainly.Shared.Interfaces;


namespace Plainly.Api.Exceptions;

public class NotFoundException : Exception, IHttpException
{
    public int StatusCode => StatusCodes.Status404NotFound;

    public NotFoundException() : base(Messages.NotFound)
    { }

    public NotFoundException(string? message) : base(message ?? Messages.NotFound)
    { }

    public NotFoundException(string? message, Exception? innerException) : base(message ?? Messages.NotFound, innerException)
    { }
}
