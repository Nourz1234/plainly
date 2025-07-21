using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Auth.Register;


public class RegisterAction : IAction<RegisterRequest, RegisterDTO>
{
    public string DisplayName => "Register";
    public string InternalName => throw new NotImplementedException();
    public Scopes[] RequiredScopes => [];
}