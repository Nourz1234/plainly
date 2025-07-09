using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Auth.Login;

public class LoginAction : IAction<LoginRequest>
{
    public string DisplayName => "Login";
    public string InternalName => "Auth.Login";
    public string? Claim => null;
}