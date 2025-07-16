using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Plainly.Api.Exceptions;
using Plainly.Api.Infrastructure.Jwt;
using Plainly.Api.Models;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Interfaces;
using Plainly.Shared.Responses;

namespace Plainly.Api.Actions.Auth;

public class RegisterActionHandler(UserManager<User> userManager, JwtService jwtService)
    : IActionHandler<RegisterAction, RegisterRequest, RegisterDTO>
{
    public async Task<RegisterDTO> Handle(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var registerForm = request.RegisterForm;
        var user = new User
        {
            FullName = registerForm.FullName,
            UserName = Guid.NewGuid().ToString(),
            Email = registerForm.Email,
        };
        var result = await userManager.CreateAsync(user, registerForm.Password);
        if (!result.Succeeded)
            throw new ValidationException(errors: new()
            {
                [""] = result.Errors.Select(x => new ValidationErrorDetail(x.Description, x.Code)).ToArray()
            });

        var token = await jwtService.GenerateToken(user);
        return new RegisterDTO(token);
    }
}