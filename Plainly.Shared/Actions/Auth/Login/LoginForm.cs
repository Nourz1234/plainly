using System.ComponentModel;

namespace Plainly.Shared.Actions.Auth.Login;

public record LoginForm()
{
    [DisplayName("Email Address")]
    public string Email { get; set; } = string.Empty;
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;
    [DisplayName("Remember Me")]
    public bool RememberMe { get; set; }
}
