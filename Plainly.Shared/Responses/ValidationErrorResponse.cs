using Microsoft.AspNetCore.Http;

namespace Plainly.Shared.Responses;

public class ValidationErrorResponse() : ErrorResponse(StatusCodes.Status422UnprocessableEntity)
{
    public required Dictionary<string, string[]> Errors { get; init; }
}




