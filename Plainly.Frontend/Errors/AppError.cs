namespace Plainly.Frontend.Errors;

public class AppError : Exception
{
    public AppError(string message) : base(message) { }
    public AppError(string message, Exception innerException) : base(message, innerException) {}
}