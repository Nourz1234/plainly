using Plainly.Api.Interfaces;
using Plainly.Shared.Auth.Register;

namespace Plainly.Api.Actions.Auth;


public class RegisterAction : IAction<RegisterRequest, RegisterDTO>
{
    public const string Scope = "auth.register";
    public string InternalName => Scope;
    public string DisplayName => "Register";

    public Task<RegisterDTO> Execute(RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}