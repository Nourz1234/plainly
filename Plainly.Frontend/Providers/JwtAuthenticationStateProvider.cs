using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Plainly.Frontend.Errors;
using Plainly.Frontend.Services;

namespace Plainly.Frontend.Providers;

public class JwtAuthenticationStateProvider(CurrentUserService currentUserService, ISnackbar snackbar) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            await currentUserService.LoadCurrentUserAsync();
        }
        catch (AuthError ex)
        {
            await currentUserService.ClearCurrentUserAsync();
            snackbar.Add(ex.Message, Severity.Error);
        }

        currentUserService.UserChanged += (user) =>
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        };

        return new AuthenticationState(currentUserService.CurrentUser);
    }
}