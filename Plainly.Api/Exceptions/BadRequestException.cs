using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class BadRequestException : BaseException
{
    public static readonly string DefaultMessage = "Invalid request! Please verify the data and try again.";

    public BadRequestException() : base(DefaultMessage)
    { }

    public BadRequestException(string? message) : base(message ?? DefaultMessage)
    { }

    public BadRequestException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    { }

    public override ErrorResponse ToResponse() => new(StatusCodes.Status400BadRequest) { Message = Message };
}
