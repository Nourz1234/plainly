using Microsoft.AspNetCore.Mvc;

namespace Plainly.Api.Controllers;

public record HealthResponse()
{
    public required string Status { get; init; }
    public required DateTime Timestamp { get; init; }
};

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public HealthResponse Get()
    {
        return new HealthResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
        };
    }
}
