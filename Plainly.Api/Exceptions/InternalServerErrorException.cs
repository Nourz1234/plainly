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

    public override ErrorResponse ToResponse() => new(StatusCodes.Status500InternalServerError) { Message = Message };
}
