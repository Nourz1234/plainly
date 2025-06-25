namespace Plainly.Shared.Responses;

public record ValidationErrorResponse() : ErrorResponse
{
    public required Dictionary<string, string[]> Errors { get; init; }
}




