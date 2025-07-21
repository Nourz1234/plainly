using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Auth.Login;

public class LoginAction : IAction<LoginRequest, LoginDTO>
{
    public string DisplayName => "Login";
    public string InternalName => throw new NotImplementedException();
    public Scopes[] RequiredScopes => [];
}