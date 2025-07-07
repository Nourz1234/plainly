using Plainly.Shared.Auth.Register;
using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Auth.Register;


public class RegisterAction : IAction<RegisterRequest, RegisterDTO>
{
    public const string Policy = "auth.register";
    public string Name => "Register";
    public string Claim => Policy;
}