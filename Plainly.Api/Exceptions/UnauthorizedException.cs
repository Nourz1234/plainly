using System.Net;


namespace Plainly.Api.Exceptions;

public class UnauthorizedException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;
    public override string DefaultMessage => "Unauthorized! Please log in to access this resource.";

    public UnauthorizedException() : base()
    { }

    public UnauthorizedException(string? message) : base(message)
    { }

    public UnauthorizedException(string? message, Exception? innerException) : base(message, innerException)
    { }
}
