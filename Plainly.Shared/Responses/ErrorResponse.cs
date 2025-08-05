using System.Text.Json.Serialization;

namespace Plainly.Shared.Responses;

public record ErrorResponse() : BaseResponse()
{
    public required string ErrorCode { get; init; }
    public required string Message { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ErrorDetail[]? Errors { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TraceId { get; init; }
}