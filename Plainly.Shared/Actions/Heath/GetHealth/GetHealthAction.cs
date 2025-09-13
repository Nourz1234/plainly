using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Heath.GetHealth;

public class GetHealthAction : IAction<GetHealthRequest, GetHealthDTO>
{
    public static ActionId Id { get; } = ActionId.GetHealth;
    public static string DisplayName { get; } = "Get Health";
    public static Scope[] RequiredScopes { get; } = [];
}
