using Microsoft.AspNetCore.Identity;
using Plainly.Application.Interface;
using Plainly.Domain;
using Plainly.Domain.Exceptions;
using Plainly.Domain.Interfaces;
using Plainly.Infrastructure.Extensions;
using Plainly.Infrastructure.Persistence.AppDatabase.Entities;

namespace Plainly.Infrastructure.Identity;

public class AuthService(SignInManager<User> signInManager) : IAuthService
{
    public async Task VerifyPasswordAsync(IUser user, string password)
    {
        var result = await signInManager.CheckPasswordSignInAsync(user.AsEntity(), password, false);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                throw DomainException.FromErrorCode(DomainErrorCode.UserIsLockedOut);
            if (result.IsNotAllowed)
                throw DomainException.FromErrorCode(DomainErrorCode.EmailNotConfirmed);
            throw DomainException.FromErrorCode(DomainErrorCode.InvalidLoginCredentials);
        }
    }
}