using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Infrastructure.Action;
using Plainly.Api.Infrastructure.Authorization.Attributes;
using Plainly.Shared;
using Plainly.Shared.Actions.Auth.Login;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Responses;

namespace Plainly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ActionDispatcher _ActionDispatcher) : ControllerBase
{

    [AuthorizeFor<RegisterAction>]
    [HttpPost("Register")]
    public async Task<SuccessResponse> Register([FromBody] RegisterForm form)
    {
        var result = await _ActionDispatcher.Dispatch<RegisterAction, RegisterRequest, RegisterDTO>(new RegisterRequest(form));

        return new SuccessResponse<RegisterDTO>(201) { Message = Messages.Success, Data = result };
    }

    [AuthorizeFor<RegisterAction>]
    [HttpPost("Login")]
    public async Task<SuccessResponse<LoginDTO>> Login([FromBody] LoginForm form)
    {
        var result = await _ActionDispatcher.Dispatch<LoginAction, LoginRequest, LoginDTO>(new LoginRequest(form));

        return new SuccessResponse<LoginDTO> { Message = Messages.Success, Data = result };
    }
}
