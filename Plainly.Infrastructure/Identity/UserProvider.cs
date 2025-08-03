using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Plainly.Domain.Interfaces;
using Plainly.Infrastructure.Persistence.AppDatabase.Repositories;

namespace Plainly.Infrastructure.Identity;

/// <summary>
/// Provider for the current authenticated user
/// </summary>
public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor, UserRepository userRepository)
{
    public async Task<IUser?> GetCurrentUserAsync()
    {
        if (httpContextAccessor.HttpContext is not { User: ClaimsPrincipal user })
            return null;
        return await userRepository.GetUserAsync(user);
    }

    /// <summary>
    /// In the case of impersonation, this will return the impersonator's user
    /// </summary>
    public Task<IUser?> GetCurrentRealUserAsync()
    {
        throw new NotImplementedException();
    }
}