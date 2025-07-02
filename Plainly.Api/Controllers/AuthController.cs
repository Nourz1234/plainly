using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Exceptions;
using Plainly.Api.Models;
using Plainly.Api.Services;
using Plainly.Shared;
using Plainly.Shared.DTOs;
using Plainly.Shared.Responses;

namespace Plainly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService) : ControllerBase
{
    private readonly UserManager<User> _UserManager = userManager;
    private readonly SignInManager<User> _SignInManager = signInManager;
    private readonly JwtService _JwtService = jwtService;

    [HttpPost("register")]
    public async Task<SuccessResponse> Register([FromBody] RegisterRequest request)
    {
        var user = new User { FullName = request.FullName, Email = request.Email, };
        var result = await _UserManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ValidationException(new Dictionary<string, string[]> { [""] = result.Errors.Select(x => x.Description).ToArray() });

        return new SuccessResponse(201) { Message = Messages.Success };
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
            Data = new LoginDTO(token)
        };
    }
}

// TODO: convert to forms with validation
public record RegisterRequest(string FullName, string Email, string Password);
public record LoginRequest(string Email, string Password);
