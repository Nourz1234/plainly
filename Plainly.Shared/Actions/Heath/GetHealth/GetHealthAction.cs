using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Heath.GetHealth;

public class GetHealthAction : IAction<GetHealthRequest, GetHealthDTO>
{
    public string DisplayName => "Get Health";
    public string InternalName => "Health.View";
    public Scopes[] RequiredScopes => [];
}
