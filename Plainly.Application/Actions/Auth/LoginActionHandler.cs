using Plainly.Application.Interface;
using Plainly.Domain;
using Plainly.Domain.Exceptions;
using Plainly.Domain.Interfaces.Repositories;
using Plainly.Shared.Actions.Auth.Login;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions.Auth;

public class LoginActionHandler(IUserRepository userRepository, IAuthService authService, IJwtService jwtService)
    : IActionHandler<LoginAction, LoginRequest, LoginDTO>
{
    public async Task<LoginDTO> Handle(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var loginFrom = request.LoginForm;
        var user = await userRepository.GetByEmailAsync(loginFrom.Email) ?? throw DomainException.FromErrorCode(DomainErrorCode.InvalidLoginCredentials);
        await authService.VerifyPasswordAsync(user, loginFrom.Password);

        if (!user.IsActive)
            throw DomainException.FromErrorCode(DomainErrorCode.UserIsNotActive);

        user.LastLoginAt = DateTime.UtcNow;
        await userRepository.UpdateAsync(user);

        var token = await jwtService.GenerateToken(user);
        return new LoginDTO(token);
    }
}