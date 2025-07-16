using Plainly.Shared;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException() : base(Messages.BadRequest)
    { }

    public BadRequestException(string? message) : base(message ?? Messages.BadRequest)
    { }

    public BadRequestException(string? message, Exception? innerException) : base(message ?? Messages.BadRequest, innerException)
    { }

    public override ErrorResponse ToResponse(string traceId) => new(StatusCodes.Status400BadRequest)
    {
        Message = Message,
        TraceId = traceId
    };
}
