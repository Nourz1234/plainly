using Plainly.Api.Infrastructure.ExceptionHandling;
using Plainly.Shared;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class InternalServerErrorException : BaseException
{
    public InternalServerErrorException() : base(Messages.InternalServerError)
    { }

    public InternalServerErrorException(string? message) : base(message ?? Messages.InternalServerError)
    { }

    public InternalServerErrorException(string? message, Exception? innerException) : base(message ?? Messages.InternalServerError, innerException)
    { }

    public override ErrorResponse ToResponse(string traceId) => ErrorResponse.InternalServerError().WithMessage(Message).WithTraceId(traceId).Build();
}
