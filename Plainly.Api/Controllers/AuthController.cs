using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Actions.Auth;
using Plainly.Api.Exceptions;
using Plainly.Api.Infrastructure.Action;
using Plainly.Api.Infrastructure.Authorization.Attributes;
using Plainly.Api.Models;
using Plainly.Api.Services;
using Plainly.Shared;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.DTOs;
using Plainly.Shared.Responses;

namespace Plainly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ActionHandlerFactory _ActionHandlerFactory) : ControllerBase
{

    [AuthorizeFor<RegisterAction>]
    [HttpPost("register")]
    public async Task<SuccessResponse> Register([FromBody] RegisterForm form)
    {
        var result = await _ActionHandlerFactory.GetHandler<RegisterAction, RegisterRequest, RegisterDTO>().Handle(new RegisterRequest(form));

        return new SuccessResponse<RegisterDTO>(201) { Message = Messages.Success, Data = result };
    }

    [HttpPost("login")]
    public async Task<SuccessResponse<LoginDTO>> Login([FromBody] LoginRequest request)
    {
        var user = await _UserManager.FindByEmailAsync(request.Email) ?? throw new UnauthorizedException(Messages.InvalidLoginCredentials);
        var result = await _SignInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            throw new UnauthorizedException();

        var token = _JwtService.GenerateToken(user);
        return new SuccessResponse<LoginDTO>
        {
            Message = Messages.Success,
            Data = new(token)
        };
    }
}

// TODO: convert to forms with validation
public record LoginRequest(string Email, string Password);
