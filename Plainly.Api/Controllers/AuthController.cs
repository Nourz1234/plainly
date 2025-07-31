using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Infrastructure.Actions;
using Plainly.Api.Infrastructure.Authorization;
using Plainly.Shared.Actions.Auth.Login;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Responses;

namespace Plainly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ActionDispatcher actionDispatcher) : ControllerBase
{

    [AuthorizeAction<RegisterAction>]
    [HttpPost("Register")]
    public async Task<SuccessResponse<RegisterDTO>> Register([FromBody] RegisterForm form)
    {
        var result = await actionDispatcher.Dispatch<RegisterAction, RegisterRequest, RegisterDTO>(new RegisterRequest(form));

        return SuccessResponse.Created().Build(result);
    }

    [AuthorizeAction<LoginAction>]
    [HttpPost("Login")]
    public async Task<SuccessResponse<LoginDTO>> Login([FromBody] LoginForm form)
    {
        var result = await actionDispatcher.Dispatch<LoginAction, LoginRequest, LoginDTO>(new LoginRequest(form));

        return SuccessResponse.Ok().Build(result);
    }
}
