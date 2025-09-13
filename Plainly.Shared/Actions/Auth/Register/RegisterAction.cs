using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Auth.Register;

public class RegisterAction : IAction<RegisterRequest, RegisterDTO>
{
    public static ActionId Id { get; } = ActionId.Register;
    public static string DisplayName { get; } = "Register";
    public static Scope[] RequiredScopes { get; } = [];
}