using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class InternalServerErrorException : BaseException
{
    public static readonly string DefaultMessage = "Internal server error! An unexpected error occurred.";

    public InternalServerErrorException() : base(DefaultMessage)
    { }

    public InternalServerErrorException(string? message) : base(message ?? DefaultMessage)
    { }

    public InternalServerErrorException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    { }

    public override ErrorResponse ToResponse() => new(StatusCodes.Status500InternalServerError) { Message = Message };
}
