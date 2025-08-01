using Plainly.Shared;
using Plainly.Shared.Interfaces;


namespace Plainly.Api.Exceptions;

public class UnauthorizedException : Exception, IHttpException
{
    public int StatusCode => StatusCodes.Status401Unauthorized;

    public UnauthorizedException() : base(Messages.Unauthorized)
    { }

    public UnauthorizedException(string? message) : base(message ?? Messages.Unauthorized)
    { }

    public UnauthorizedException(string? message, Exception? innerException) : base(message ?? Messages.Unauthorized, innerException)
    { }
}
