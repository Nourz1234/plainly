using Microsoft.AspNetCore.Http;

namespace Plainly.Shared.Responses;

public class ValidationErrorResponse() : ErrorResponse(StatusCodes.Status422UnprocessableEntity)
{
    public required Dictionary<string, ValidationErrorDetail[]> Errors { get; init; }
}

public record ValidationErrorDetail(string Message, string ErrorCode);