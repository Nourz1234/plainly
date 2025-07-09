namespace Plainly.Shared.Actions.Heath.GetHealth;

public record GetHealthDTO()
{
    public required string Status { get; init; }
    public required DateTime Timestamp { get; init; }
};