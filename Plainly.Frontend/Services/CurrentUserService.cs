using System.Security.Claims;
using Blazored.LocalStorage;

namespace Plainly.Frontend.Services;

public class CurrentUserService(ILocalStorageService localStorageService, JwtTokenValidationService jwtTokenValidationService)
{
    private static readonly ClaimsPrincipal Anonymous = new();

    public async Task LoadCurrentUserAsync()
    {
        var token = await localStorageService.GetItemAsStringAsync("token");
        if (token is null)
            ClearCurrentUserInternal();
        else
            SetCurrentUserInternalAsync(token);
    }

    public async Task SetCurrentUserAsync(string token)
    {
        SetCurrentUserInternalAsync(token);
        await localStorageService.SetItemAsStringAsync("token", token);
    }

    public async Task ClearCurrentUserAsync()
    {
        await localStorageService.RemoveItemAsync("token");
        ClearCurrentUserInternal();
    }

    private void SetCurrentUserInternalAsync(string token)
    {
        var claimsPrincipal = jwtTokenValidationService.ValidateTokenAsync(token);
        _CurrentUser = claimsPrincipal;
        Token = token;
    }

    private void ClearCurrentUserInternal()
    {
        _CurrentUser = Anonymous;
        Token = null;
    }

    private ClaimsPrincipal _CurrentUser = Anonymous;
    public ClaimsPrincipal CurrentUser
    {
        get => _CurrentUser;
        private set
        {
            if (_CurrentUser != value)
            {
                _CurrentUser = value;
                UserChanged?.Invoke(_CurrentUser);
            }
        }
    }

    public string? Token { get; private set; }
    public bool IsAuthenticated => _CurrentUser.Identity is { IsAuthenticated: true };
    public string FullName => _CurrentUser.Identity?.Name ?? "Anonymous";
    public string Email => _CurrentUser.FindFirst(ClaimTypes.Email)?.Value ?? "Anonymous";

    public event Action<ClaimsPrincipal>? UserChanged;
}