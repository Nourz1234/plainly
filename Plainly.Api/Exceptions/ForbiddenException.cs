using Plainly.Shared;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class ForbiddenException : BaseException
{
    public ForbiddenException() : base(Messages.Forbidden)
    { }

    public ForbiddenException(string? message) : base(message ?? Messages.Forbidden)
    { }

    public ForbiddenException(string? message, Exception? innerException) : base(message ?? Messages.Forbidden, innerException)
    { }

    public override ErrorResponse ToResponse(string traceId) => new(StatusCodes.Status403Forbidden)
    {
        Message = Message,
        TraceId = traceId
    };
}
