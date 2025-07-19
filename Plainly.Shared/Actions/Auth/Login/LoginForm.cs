using System.ComponentModel;

namespace Plainly.Shared.Actions.Auth.Login;

public record LoginForm()
{
    [DisplayName("Email Address")]
    public string Email { get; init; } = string.Empty;
    [DisplayName("Password")]
    public string Password { get; init; } = string.Empty;
    [DisplayName("Remember Me")]
    public bool RememberMe { get; init; }
}
