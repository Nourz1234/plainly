using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.User.EditProfile;

public class EditProfileAction : IAction<EditProfileRequest>
{
    public string DisplayName => "Edit Profile";

    public string InternalName => throw new NotImplementedException();

    public Scopes[] RequiredScopes => [Scopes.Profile];
}
