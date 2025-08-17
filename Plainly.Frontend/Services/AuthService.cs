namespace Plainly.Frontend.Services;

public class AuthService(ApiService apiService, CurrentUserService currentUserService)
{
    public async Task LoginAsync(string email, string password)
    {
        var result = await apiService.LoginAsync(new() { Email = email, Password = password });
        await currentUserService.SetCurrentUserAsync(result.Token);
    }

    public async Task Logout() => await currentUserService.ClearCurrentUserAsync();
}