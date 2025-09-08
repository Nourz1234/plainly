using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Plainly.Frontend.Errors;
using Plainly.Frontend.Services;

namespace Plainly.Frontend.Providers;

public class JwtAuthenticationStateProvider(CurrentUserService currentUserService, LocalStorageWatcher localStorageWatcher, ISnackbar snackbar) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await LoadCurrentUserAsync();

        currentUserService.UserChanged += (user) =>
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        };

        // watch changes from other tabs
        await localStorageWatcher.StartWatchingAsync();
        localStorageWatcher.StorageChanged += async (key, oldValue, newValue) =>
        {
            if (key == "token")
            {
                await LoadCurrentUserAsync();
            }
        };

        return new AuthenticationState(currentUserService.CurrentUser);
    }

    private async Task LoadCurrentUserAsync()
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
    }
}