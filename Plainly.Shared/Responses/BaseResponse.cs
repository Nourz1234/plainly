
namespace Plainly.Shared.Responses;

public abstract record BaseResponse()
{
    public required bool Success { get; init; }
    public required int StatusCode { get; init; }
}
