using System.Security.Claims;
using Plainly.Application.Interface;
using Plainly.Domain.Interfaces.Repositories;
using Plainly.Shared;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Interfaces;
using Plainly.Shared.Extensions;

namespace Plainly.Application.Actions.Auth;

public class RegisterActionHandler(IUserRepository userRepository, IJwtService jwtService)
    : IActionHandler<RegisterAction, RegisterRequest, RegisterDTO>
{
    public async Task<RegisterDTO> Handle(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var registerForm = request.RegisterForm;
        var user = await userRepository.CreateAsync(registerForm.FullName, registerForm.Email, registerForm.Password);

        await userRepository.AddClaimAsync(user,
            new Claim("scopes", Scopes.Profile.GetEnumMemberValue())
        );

        var token = await jwtService.GenerateToken(user);
        return new RegisterDTO(token);
    }
}