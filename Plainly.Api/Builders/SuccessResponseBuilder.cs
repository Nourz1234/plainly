using Plainly.Shared.Responses;

namespace Plainly.Api.Builders;


public class SuccessResponseBuilder
{

    public static SuccessResponseBuilder Ok() => new(StatusCodes.Status200OK, Domain.DomainMessages.Success);
    public static SuccessResponseBuilder Created() => new(StatusCodes.Status201Created, Domain.DomainMessages.Success);

    protected readonly bool _Success = true;
    protected readonly int _StatusCode;
    protected string _Message;

    private SuccessResponseBuilder(int statusCode, string message)
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
