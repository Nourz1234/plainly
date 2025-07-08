using Microsoft.AspNetCore.Identity;
using Plainly.Api.Exceptions;
using Plainly.Api.Models;
using Plainly.Api.Services;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions.Auth;

public class RegisterActionHandler(UserManager<User> _UserManager, JwtService _JwtService)
    : IActionHandler<RegisterAction, RegisterRequest, RegisterDTO>
{
    public async Task<RegisterDTO> Handle(RegisterRequest request)
    {
        var registerForm = request.RegisterForm;
        var user = new User { FullName = registerForm.FullName, Email = registerForm.Email, };
        var result = await _UserManager.CreateAsync(user, registerForm.Password);

        if (!result.Succeeded)
            throw new ValidationException(new Dictionary<string, string[]> { [""] = result.Errors.Select(x => x.Description).ToArray() });

        var token = _JwtService.GenerateToken(user);
        return new RegisterDTO(token);
    }
}