using Plainly.Shared.Responses;

namespace Plainly.Shared.Builders;

public class ErrorResponseBuilder
{
    protected readonly bool _Success = false;
    protected readonly int _StatusCode;
    protected string _Message;
    protected ErrorDetail[]? _Errors;
    protected string? _TraceId;

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

    public ErrorResponseBuilder WithTraceId(string traceId)
    {
        _TraceId = traceId;
        return this;
    }

    public ErrorResponse Build() => new() { Success = _Success, StatusCode = _StatusCode, Message = _Message, Errors = _Errors, TraceId = _TraceId };
}