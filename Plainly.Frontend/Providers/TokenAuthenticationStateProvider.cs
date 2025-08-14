using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plainly.Frontend.Services;

namespace Plainly.Frontend.Providers;

public class TokenAuthenticationStateProvider(JwtValidator jwtValidator, CurrentUserService currentUserService, IJSRuntime jsRuntime) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal Anonymous = new();

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(Anonymous);
        }

        currentUserService.UserChanged += (user) =>
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        };

        var claimsPrincipal = await jwtValidator.ValidateTokenAsync(token);
        return new AuthenticationState(claimsPrincipal ?? Anonymous);
    }

    public void SetAuthenticationStateAsync(ClaimsPrincipal claimsPrincipal)
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}