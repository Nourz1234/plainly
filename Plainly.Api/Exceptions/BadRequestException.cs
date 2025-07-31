using Plainly.Api.Infrastructure.ExceptionHandling;
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

    public override ErrorResponse ToResponse(string traceId) => ErrorResponse.BadRequest().WithMessage(Message).WithTraceId(traceId).Build();
}
