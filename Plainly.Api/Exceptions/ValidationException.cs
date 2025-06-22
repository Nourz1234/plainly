using System.Net;


namespace Plainly.Api.Exceptions;

public class ValidationException : BaseException
{
    public override int StatusCode => (int)HttpStatusCode.UnprocessableContent;
    public override string DefaultMessage => "Validation failed! The input data was not valid.";

    public Dictionary<string, string[]> Errors { get; } = [];


    public ValidationException() : base()
    { }

    public ValidationException(Dictionary<string, string[]> errors) : base()
    {
        Errors = errors;
    }

    public ValidationException(string? message) : base(message)
    { }

    public ValidationException(string? message, Dictionary<string, string[]> errors) : base(message)
    {
        Errors = new Dictionary<string, string[]>(errors);
    }

    public ValidationException(string? message, Exception? innerException) : base(message, innerException)
    { }

    public ValidationException(string? message, Exception? innerException, Dictionary<string, string[]> errors) : base(message, innerException)
    {
        Errors = new Dictionary<string, string[]>(errors);
    }

    public override object GetResult() => new
    {
        StatusCode = (int)HttpStatusCode.UnprocessableContent,
        Status = "error",
        Message = MessageOrDefault(),
        Errors
    };
}
