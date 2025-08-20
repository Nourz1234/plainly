namespace Plainly.Frontend.Errors;

public class AuthError : AppError
{
    public AuthError(string message) : base(message) { }
    public AuthError(string message, Exception innerException) : base(message, innerException) { }
}