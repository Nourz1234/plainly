using Plainly.Shared.Actions.Heath.GetHealth;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions.Health;

public class GetHealthActionHandler : IActionHandler<GetHealthAction, GetHealthRequest, GetHealthDTO>
{
    public Task<GetHealthDTO> Handle(GetHealthRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new GetHealthDTO
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
        });
    }
}