using Plainly.Api.Infrastructure.ExceptionHandling;
using Plainly.Shared;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException() : base(Messages.Unauthorized)
    { }

    public UnauthorizedException(string? message) : base(message ?? Messages.Unauthorized)
    { }

    public UnauthorizedException(string? message, Exception? innerException) : base(message ?? Messages.Unauthorized, innerException)
    { }

    public override ErrorResponse ToResponse(string traceId) => ErrorResponse.Unauthorized().WithMessage(Message).WithTraceId(traceId).Build();
}
