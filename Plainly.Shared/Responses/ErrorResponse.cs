using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Responses;

public record ErrorResponse() : IResponse
{
    public bool Success => false;
    public required string Message { get; init; }
}




