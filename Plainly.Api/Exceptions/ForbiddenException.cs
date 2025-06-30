using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class ForbiddenException : BaseException
{
    public static readonly string DefaultMessage = "Forbidden! You do not have permission to access this resource.";

    public ForbiddenException() : base(DefaultMessage)
    { }

    public ForbiddenException(string? message) : base(message ?? DefaultMessage)
    { }

    public ForbiddenException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    { }

    public override ErrorResponse ToResponse() => new(StatusCodes.Status403Forbidden) { Message = Message };

}
