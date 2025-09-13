using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.User.ViewProfile;

public class ViewProfileAction : IAction<ViewProfileRequest, ViewProfileDTO>
{
    public static ActionId Id { get; } = ActionId.ViewProfile;
    public static string DisplayName { get; } = "View Profile";
    public static Scope[] RequiredScopes { get; } = [Scope.Profile];
}
