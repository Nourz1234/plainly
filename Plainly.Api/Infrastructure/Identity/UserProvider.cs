using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Plainly.Api.Infrastructure.Identity;

public class UserProvider<T>(IHttpContextAccessor httpContextAccessor, UserManager<T> userManager)
    where T : class
{
    public async Task<T?> GetCurrentAsync()
    {
        if (httpContextAccessor.HttpContext is not { User: ClaimsPrincipal user })
            return null;
        return await userManager.GetUserAsync(user);
    }

    /// <summary>
    /// In the case of impersonation, this will return the impersonator's user
    /// </summary>
    public Task<T?> GetRealAsync()
    {
        throw new NotImplementedException();
    }
}