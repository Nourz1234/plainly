using Microsoft.AspNetCore.Identity;
using Plainly.Api.Entities;
using Plainly.Api.Exceptions;
using Plainly.Api.Infrastructure.Jwt;
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

        if (!user.IsActive)
            throw new UnauthorizedException(Messages.UserIsNotActive);

        var token = await jwtService.GenerateToken(user);
        return new LoginDTO(token);
    }
}