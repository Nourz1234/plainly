using System.Net;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class InternalServerErrorException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.InternalServerError;
    public static readonly string DefaultMessage = "Internal server error! An unexpected error occurred.";

    public InternalServerErrorException() : base(DefaultMessage)
    { }

    public InternalServerErrorException(string? message) : base(message ?? DefaultMessage)
    { }

    public InternalServerErrorException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    { }

    public override ErrorResponse ToResponse() => new() { Message = Message };
}
