using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.User.ViewProfile;

public class ViewProfileAction : IAction<ViewProfileRequest, ViewProfileDTO>
{
    public string DisplayName => "View Profile";

    public string InternalName => throw new NotImplementedException();

    public Scopes[] RequiredScopes => [Scopes.Profile];
}
