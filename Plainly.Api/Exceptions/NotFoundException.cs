using System.Net;


namespace Plainly.Api.Exceptions;

public class NotFoundException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;
    public override string DefaultMessage => "Not found! The requested resource could not be found.";

    public NotFoundException() : base()
    { }

    public NotFoundException(string? message) : base(message)
    { }

    public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
    { }
}
