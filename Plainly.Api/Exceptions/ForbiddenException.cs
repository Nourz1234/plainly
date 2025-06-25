using System.Net;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class ForbiddenException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.Forbidden;
    public static readonly string DefaultMessage = "Forbidden! You do not have permission to access this resource.";

    public ForbiddenException() : base(DefaultMessage)
    { }

    public ForbiddenException(string? message) : base(message ?? DefaultMessage)
    { }

    public ForbiddenException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    { }

    public override ErrorResponse ToResponse() => new() { Message = Message };

}
