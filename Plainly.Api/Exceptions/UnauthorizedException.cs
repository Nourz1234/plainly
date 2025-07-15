using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class UnauthorizedException : BaseException
{
    public static readonly string DefaultMessage = "Unauthorized! Please log in to access this resource.";

    public UnauthorizedException() : base(DefaultMessage)
    { }

    public UnauthorizedException(string? message) : base(message ?? DefaultMessage)
    { }

    public UnauthorizedException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    { }

    public override ErrorResponse ToResponse(string traceId) => new(StatusCodes.Status401Unauthorized)
    {
        Message = Message,
        TraceId = traceId
    };
}
