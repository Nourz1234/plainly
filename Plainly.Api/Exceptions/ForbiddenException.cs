using System.Net;


namespace Plainly.Api.Exceptions;

public class ForbiddenException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.Forbidden;
    public override string DefaultMessage => "Forbidden! You do not have permission to access this resource.";

    public ForbiddenException() : base()
    { }

    public ForbiddenException(string? message) : base(message)
    { }

    public ForbiddenException(string? message, Exception? innerException) : base(message, innerException)
    { }
}
