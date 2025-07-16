using Microsoft.AspNetCore.Identity;
using Plainly.Api.Exceptions;
using Plainly.Api.Models;
using Plainly.Api.Services;
using Plainly.Shared;
using Plainly.Shared.Actions.Auth.Login;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions.Auth;

public class LoginActionHandler(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService)
    : IActionHandler<LoginAction, LoginRequest, LoginDTO>
{
    public async Task<LoginDTO> Handle(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var loginFrom = request.LoginForm;
        var user = await userManager.FindByEmailAsync(loginFrom.Email) ?? throw new UnauthorizedException(Messages.InvalidLoginCredentials);
        var result = await signInManager.CheckPasswordSignInAsync(user, loginFrom.Password, false);
        if (!result.Succeeded)
            throw new UnauthorizedException(Messages.InvalidLoginCredentials);

        var token = jwtService.GenerateToken(user);
        return new LoginDTO(token);
    }
}