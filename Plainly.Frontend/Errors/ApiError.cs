namespace Plainly.Frontend.Errors;

public class ApiError : AppError
{
    public ApiError(string message) : base(message) { }
    public ApiError(string message, Exception innerException) : base(message, innerException) { }
}