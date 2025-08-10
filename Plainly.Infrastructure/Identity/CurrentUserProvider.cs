using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Plainly.Application.Interface;
using Plainly.Application.Interface.Repositories;
using Plainly.Domain.Interfaces;

namespace Plainly.Infrastructure.Identity;

/// <summary>
/// Provider for the current authenticated user
/// </summary>
public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository) : ICurrentUserProvider
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
    public Task<IUser?> GetCurrentImpersonatorUserAsync()
    {
        throw new NotImplementedException();
    }
}