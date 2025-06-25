namespace Plainly.Shared.DTOs;

public record HealthDTO()
{
    public required string Status { get; init; }
    public required DateTime Timestamp { get; init; }
};