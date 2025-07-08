using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Auth.Register;


public class RegisterAction : IAction<RegisterRequest>
{
    public string DisplayName => "Register";
    public string InternalName => "auth.register";
    public string Claim => InternalName;
}