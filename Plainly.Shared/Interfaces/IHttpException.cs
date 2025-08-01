namespace Plainly.Shared.Interfaces;

public interface IHttpException
{
    int StatusCode { get; }
    string Message { get; }
}