using System.Net;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class UnauthorizedException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;
    public static readonly string DefaultMessage = "Unauthorized! Please log in to access this resource.";

    public UnauthorizedException() : base(DefaultMessage)
    { }

    public UnauthorizedException(string? message) : base(message ?? DefaultMessage)
    { }

    public UnauthorizedException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    { }

    public override ErrorResponse ToResponse() => new() { Message = Message };
}
