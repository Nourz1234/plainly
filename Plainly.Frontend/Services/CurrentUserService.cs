using System.Security.Claims;
using Blazored.LocalStorage;
using Plainly.Shared.Extensions;
using Plainly.Shared.Interfaces;

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
            await SetCurrentUserInternalAsync(token);
    }

    public async Task SetCurrentUserAsync(string token)
    {
        await SetCurrentUserInternalAsync(token);
        await localStorageService.SetItemAsStringAsync("token", token);
    }

    public async Task ClearCurrentUserAsync()
    {
        await localStorageService.RemoveItemAsync("token");
        ClearCurrentUserInternal();
    }

    private async Task SetCurrentUserInternalAsync(string token)
    {
        var claimsPrincipal = await jwtTokenValidationService.ValidateTokenAsync(token);
        CurrentUser = claimsPrincipal;
        Token = token;
    }

    private void ClearCurrentUserInternal()
    {
        CurrentUser = Anonymous;
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


    public bool CanPerformAction<TAction>() where TAction : IAction, new()
    {
        return CanPerformAction(new TAction());
    }

    public bool CanPerformAction(IAction action)
    {
        var actionScopes = action.RequiredScopes.Select(x => x.GetEnumMemberValue()).ToArray();
        if (actionScopes.Length == 0)
        {
            return true;
        }

        if (_CurrentUser.Identity is null || !_CurrentUser.Identity.IsAuthenticated)
        {
            return false;
        }

        // Admin has access to all!
        if (_CurrentUser.IsInRole("Admin"))
        {
            return true;
        }

        var userScopes = _CurrentUser.FindAll("scopes").Select(c => c.Value).ToArray();
        bool userHasScope(string scope) => userScopes.Any(userScope => userScope == scope || scope.StartsWith(userScope + "."));

        if (!actionScopes.All(userHasScope))
        {
            return false;
        }

        return true;
    }

    public event Action<ClaimsPrincipal>? UserChanged;
}