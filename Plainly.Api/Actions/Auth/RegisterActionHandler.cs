using Plainly.Api.Interfaces;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Auth.Register;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions.Auth;


public class RegisterActionHandler : IActionHandler<RegisterAction, RegisterRequest, RegisterDTO>
{
    public Task<RegisterDTO> Handle(RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}