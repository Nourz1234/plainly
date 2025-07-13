using System.ComponentModel;

namespace Plainly.Shared.Actions.Auth.Register;

public record RegisterForm()
{
    [DisplayName("Full Name")]
    public required string FullName { get; init; }
    [DisplayName("Email Address")]
    public required string Email { get; init; }
    [DisplayName("Password")]
    public required string Password { get; init; }
    [DisplayName("Confirm Password")]
    public required string ConfirmPassword { get; init; }
}
