using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class NotFoundException : BaseException
{
    public static readonly string DefaultMessage = "Not found! The requested resource could not be found.";

    public NotFoundException() : base(DefaultMessage)
    { }

    public NotFoundException(string? message) : base(message ?? DefaultMessage)
    { }

    public NotFoundException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    { }

    public override ErrorResponse ToResponse(string traceId) => new(StatusCodes.Status404NotFound)
    {
        Message = Message,
        TraceId = traceId
    };
}
