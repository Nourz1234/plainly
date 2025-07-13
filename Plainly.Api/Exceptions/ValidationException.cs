using Plainly.Shared;
using Plainly.Shared.Responses;


namespace Plainly.Api.Exceptions;

public class ValidationException : BaseException
{
    public Dictionary<string, ValidationErrorDetail[]> Errors { get; }


    public ValidationException() : base(Messages.ValidationError)
    {
        Errors = [];
    }

    public ValidationException(Dictionary<string, ValidationErrorDetail[]> errors) : base(Messages.ValidationError)
    {
        Errors = new Dictionary<string, ValidationErrorDetail[]>(errors);
    }

    public ValidationException(string? message) : base(message ?? Messages.ValidationError)
    {
        Errors = [];
    }

    public ValidationException(string? message, Dictionary<string, ValidationErrorDetail[]> errors) : base(message ?? Messages.ValidationError)
    {
        Errors = new Dictionary<string, ValidationErrorDetail[]>(errors);
    }

    public ValidationException(string? message, Exception? innerException) : base(message ?? Messages.ValidationError, innerException)
    {
        Errors = [];
    }

    public ValidationException(string? message, Dictionary<string, ValidationErrorDetail[]> errors, Exception? innerException) : base(message ?? Messages.ValidationError, innerException)
    {
        Errors = new Dictionary<string, ValidationErrorDetail[]>(errors);
    }

    public override ErrorResponse ToResponse() => new ValidationErrorResponse { Message = Message, Errors = Errors };
}
