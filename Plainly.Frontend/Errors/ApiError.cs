using MudBlazor;
using Plainly.Shared.Responses;

namespace Plainly.Frontend.Errors;

public class ApiError : AppError
{
    public ErrorDetail[]? Errors { get; init; }

    public string? TraceId { get; init; }

    public ApiError(string message) : base(message) { }
    public ApiError(string message, Exception innerException) : base(message, innerException) { }
}