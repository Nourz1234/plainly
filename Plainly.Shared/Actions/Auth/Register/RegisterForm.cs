using System.ComponentModel;

namespace Plainly.Shared.Actions.Auth.Register;

public record RegisterForm()
{
    [DisplayName("Full Name")]
    public string FullName { get; init; } = string.Empty;
    [DisplayName("Email Address")]
    public string Email { get; init; } = string.Empty;
    [DisplayName("Password")]
    public string Password { get; init; } = string.Empty;
    [DisplayName("Confirm Password")]
    public string ConfirmPassword { get; init; } = string.Empty;
}
