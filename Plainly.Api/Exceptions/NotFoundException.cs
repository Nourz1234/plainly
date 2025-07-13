using Plainly.Shared;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException() : base(Messages.NotFound)
    { }

    public NotFoundException(string? message) : base(message ?? Messages.NotFound)
    { }

    public NotFoundException(string? message, Exception? innerException) : base(message ?? Messages.NotFound, innerException)
    { }

    public override ErrorResponse ToResponse() => new(StatusCodes.Status404NotFound) { Message = Message };
}
