using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Auth.Register;


public class RegisterAction : IAction<RegisterRequest>
{
    public string DisplayName => "Register";
    public string InternalName => "Auth.Register";
    public string? Claim => null;
}