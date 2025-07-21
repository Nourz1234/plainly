using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Plainly.Api.Entities;
using Plainly.Api.Exceptions;
using Plainly.Api.Extensions;
using Plainly.Api.Infrastructure.Jwt;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Interfaces;
using Plainly.Shared.Responses;

namespace Plainly.Api.Actions.Auth;

public class RegisterActionHandler(UserManager<Entities.User> userManager, JwtService jwtService)
    : IActionHandler<RegisterAction, RegisterRequest, RegisterDTO>
{
    public async Task<RegisterDTO> Handle(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var registerForm = request.RegisterForm;
        var user = new Entities.User
        {
            FullName = registerForm.FullName,
            Email = registerForm.Email,
            IsActive = true,
        };
        var result = await userManager.CreateAsync(user, registerForm.Password);
        result.ThrowIfFailed();

        var token = await jwtService.GenerateToken(user);
        return new RegisterDTO(token);
    }
}