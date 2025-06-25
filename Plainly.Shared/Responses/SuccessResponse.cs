using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Responses;

public record SuccessResponse() : IResponse
{
    public bool Success => true;
    public required string Message { get; init; }
}

public record SuccessResponse<T>() : SuccessResponse
{
    public required T Data { get; init; }
}




