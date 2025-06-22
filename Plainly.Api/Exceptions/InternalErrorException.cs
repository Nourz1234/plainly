using System.Net;


namespace Plainly.Api.Exceptions;

public class InternalErrorException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.InternalServerError;
    public override string DefaultMessage => "Internal server error! An unexpected error occurred.";

    public InternalErrorException() : base()
    { }

    public InternalErrorException(string? message) : base(message)
    { }

    public InternalErrorException(string? message, Exception? innerException) : base(message, innerException)
    { }
}
