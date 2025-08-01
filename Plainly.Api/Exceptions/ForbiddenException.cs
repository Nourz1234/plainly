using Plainly.Shared;
using Plainly.Shared.Interfaces;


namespace Plainly.Api.Exceptions;

public class ForbiddenException : Exception, IHttpException
{
    public int StatusCode => StatusCodes.Status403Forbidden;

    public ForbiddenException() : base(Messages.Forbidden)
    { }

    public ForbiddenException(string? message) : base(message ?? Messages.Forbidden)
    { }

    public ForbiddenException(string? message, Exception? innerException) : base(message ?? Messages.Forbidden, innerException)
    { }
}
