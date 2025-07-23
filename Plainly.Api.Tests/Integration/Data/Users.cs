using Plainly.Shared;

namespace Plainly.Api.Tests.Integration.Data;

public record User(string FullName, string Email, string Password, Scopes[]? Scopes = null, bool IsActive = true);

public static class Users
{
    public static readonly User AdminUser = new("Admin", "admin@plainly.com", "123456");
    public static readonly User TestUser = new("Test", "test@plainly.com", "123456", [Scopes.Profile]);
    public static readonly User NoScopesUser = new("No Scopes User", "no-scopes@plainly.com", "123456");
    public static readonly User InActiveUser = new("InActive", "in-active@plainly.com", "123456", IsActive: false);
}