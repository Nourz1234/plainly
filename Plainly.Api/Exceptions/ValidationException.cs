using System.Net;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class ValidationException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.UnprocessableContent;
    public static readonly string DefaultMessage = "Validation failed! The input data was not valid.";

    public Dictionary<string, string[]> Errors { get; }


    public ValidationException() : base(DefaultMessage)
    {
        Errors = [];
    }

    public ValidationException(Dictionary<string, string[]> errors) : base(DefaultMessage)
    {
        Errors = new Dictionary<string, string[]>(errors);
    }

    public ValidationException(string? message) : base(message ?? DefaultMessage)
    {
        Errors = [];
    }

    public ValidationException(string? message, Dictionary<string, string[]> errors) : base(message ?? DefaultMessage)
    {
        Errors = new Dictionary<string, string[]>(errors);
    }

    public ValidationException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    {
        Errors = [];
    }

    public ValidationException(string? message, Exception? innerException, Dictionary<string, string[]> errors) : base(message ?? DefaultMessage, innerException)
    {
        Errors = new Dictionary<string, string[]>(errors);
    }

    public override ErrorResponse ToResponse() => new ValidationErrorResponse { Message = Message, Errors = Errors };
}
