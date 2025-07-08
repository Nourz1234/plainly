namespace Plainly.Shared.Actions.Auth.Register;

public record RegisterForm(string FullName, string Email, string Password, string ConfirmPassword);