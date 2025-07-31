using Plainly.Api.Exceptions;
using Plainly.Api.Infrastructure.Identity;

namespace Plainly.Api.Extensions;

public static class UserProviderExtensions
{
    public static async Task<T> GetCurrentOrFailAsync<T>(this UserProvider<T> userProvider) where T : class
        => await userProvider.GetCurrentAsync() ?? throw new UnauthorizedException();
}