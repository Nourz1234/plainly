using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Auth.Login;

public class LoginAction : IAction<LoginRequest, LoginDTO>
{
    public static ActionId Id { get; } = ActionId.Login;
    public static string DisplayName { get; } = "Login";
    public static Scope[] RequiredScopes { get; } = [];
}