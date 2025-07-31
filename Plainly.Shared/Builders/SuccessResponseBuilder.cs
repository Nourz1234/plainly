using Plainly.Shared.Responses;

namespace Plainly.Shared.Builders;


public class SuccessResponseBuilder
{
    protected readonly bool _Success = true;
    protected readonly int _StatusCode;
    protected string _Message;
    protected ErrorDetail[]? _Errors;
    protected string? _TraceId;

    internal SuccessResponseBuilder(int statusCode, string message)
    {
        _StatusCode = statusCode;
        _Message = message;
    }

    public SuccessResponseBuilder WithMessage(string message)
    {
        _Message = message;
        return this;
    }

    public SuccessResponse Build() => new() { Success = _Success, StatusCode = _StatusCode, Message = _Message };
    public SuccessResponse<T> Build<T>(T data) => new() { Success = _Success, StatusCode = _StatusCode, Message = _Message, Data = data };

}
