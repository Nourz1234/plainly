using System.Net;


namespace Plainly.Api.Exceptions;

public class BadRequestException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;
    public override string DefaultMessage => "Invalid request! Please verify the data and try again.";

    public BadRequestException() : base()
    { }

    public BadRequestException(string? message) : base(message)
    { }

    public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
    { }
}
