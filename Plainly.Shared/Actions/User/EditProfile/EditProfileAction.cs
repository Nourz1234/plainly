using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.User.EditProfile;

public class EditProfileAction : IAction<EditProfileRequest, EditProfileDTO>
{
    public static ActionId Id { get; } = ActionId.EditProfile;
    public static string DisplayName { get; } = "Edit Profile";
    public static Scope[] RequiredScopes { get; } = [Scope.Profile];
}
