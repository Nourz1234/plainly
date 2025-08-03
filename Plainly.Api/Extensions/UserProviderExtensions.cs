using Plainly.Api.Exceptions;
using Plainly.Infrastructure.Identity;

namespace Plainly.Api.Extensions;

public static class UserProviderExtensions
{
    public static async Task<T> GetCurrentOrFailAsync<T>(this CurrentUserProvider<T> userProvider) where T : class
        => await userProvider.GetCurrentAsync() ?? throw new UnauthorizedException();
}