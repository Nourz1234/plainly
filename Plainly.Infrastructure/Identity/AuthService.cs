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
    public async Task CheckPasswordAsync(IUser user, string password)
    {
        var result = await signInManager.CheckPasswordSignInAsync(user.AsEntity(), password, false);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                throw new DomainException(DomainErrorCode.UserIsLockedOut);
            if (result.IsNotAllowed)
                throw new DomainException(DomainErrorCode.EmailNotConfirmed);
            throw new DomainException(DomainErrorCode.InvalidLoginCredentials);
        }
    }
}