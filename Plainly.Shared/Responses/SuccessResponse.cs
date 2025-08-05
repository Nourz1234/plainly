namespace Plainly.Shared.Responses;

public record SuccessResponse() : BaseResponse()
{
    public required string Message { get; init; }
}

public record SuccessResponse<T>() : SuccessResponse()
{
    public required T Data { get; init; }
}
