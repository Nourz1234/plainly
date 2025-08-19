using System.ComponentModel;

namespace Plainly.Shared.Actions.Auth.Register;

public record RegisterForm()
{
    [DisplayName("Full Name")]
    public string FullName { get; set; } = string.Empty;
    [DisplayName("Email Address")]
    public string Email { get; set; } = string.Empty;
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;
    [DisplayName("Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
