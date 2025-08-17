using System.Security.Claims;
using Blazored.LocalStorage;

namespace Plainly.Frontend.Services;

public class CurrentUserService(ILocalStorageService localStorageService, JwtTokenValidationService jwtTokenValidationService)
{
    private static readonly ClaimsPrincipal Anonymous = new();

    public async Task InitializeAsync()
    {
        var token = await localStorageService.GetItemAsStringAsync("token");
        if (token is not null)
        {
            await SetCurrentUserAsync(token);
        }
    }

    public async Task SetCurrentUserAsync(string token)
    {
        var claimsPrincipal = await jwtTokenValidationService.ValidateTokenAsync(token);
        _CurrentUser = claimsPrincipal;
        await localStorageService.SetItemAsStringAsync("token", token);
        UserChanged?.Invoke(_CurrentUser);
    }

    public async Task ClearCurrentUserAsync()
    {
        await localStorageService.RemoveItemAsync("token");
        _CurrentUser = Anonymous;
        UserChanged?.Invoke(_CurrentUser);
    }

    private ClaimsPrincipal _CurrentUser = Anonymous;
    public ClaimsPrincipal CurrentUser => _CurrentUser;

    public event Action<ClaimsPrincipal>? UserChanged;
}