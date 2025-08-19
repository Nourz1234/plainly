namespace Plainly.Frontend.Errors;

public class ApiError : Exception
{
    public ApiError(string message) : base(message) { }
    public ApiError(string message, Exception innerException) : base(message, innerException) { }
}