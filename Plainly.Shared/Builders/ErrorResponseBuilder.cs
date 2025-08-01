using Plainly.Shared.Responses;

namespace Plainly.Shared.Builders;

public class ErrorResponseBuilder
{
    protected readonly bool _Success = false;
    protected readonly int _StatusCode;
    protected string _Message;
    protected string? _ErrorCode;
    protected ErrorDetail[]? _Errors;

    internal ErrorResponseBuilder(int statusCode, string message)
    {
        _StatusCode = statusCode;
        _Message = message;
    }

    public ErrorResponseBuilder WithMessage(string message)
    {
        _Message = message;
        return this;
    }

    public ErrorResponseBuilder WithErrors(ErrorDetail[] errors)
    {
        _Errors = errors;
        return this;
    }


    public ErrorResponse Build(string traceId) => new() { Success = _Success, StatusCode = _StatusCode, Message = _Message, Errors = _Errors, TraceId = traceId };
}