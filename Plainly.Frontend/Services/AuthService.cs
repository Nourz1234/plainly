namespace Plainly.Frontend.Services;

public class AuthService(JwtValidator jwtValidator, CurrentUserService currentUserService)
{
    public async Task AuthenticateUser(string token)
    {
        currentUserService.CurrentUser = await jwtValidator.ValidateTokenAsync(token);
    }
}