using Microsoft.AspNetCore.Components.Authorization;
using Plainly.Frontend.Services;

namespace Plainly.Frontend.Providers;

public class JwtAuthenticationStateProvider(CurrentUserService currentUserService) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await currentUserService.InitializeAsync();
        currentUserService.UserChanged += (user) =>
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        };

        return new AuthenticationState(currentUserService.CurrentUser);
    }
}