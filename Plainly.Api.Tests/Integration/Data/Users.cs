using Bogus;
using Plainly.Shared;

namespace Plainly.Api.Tests.Integration.Data;

public record User
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public Scopes[]? Scopes { get; init; } = null;
    public bool IsActive { get; init; } = true;

    public static User Generate(string password = "123456", Scopes[]? scopes = null, bool isActive = true) => new Faker<User>()
        .RuleFor(x => x.FullName, f => f.Person.FullName)
        .RuleFor(x => x.Email, f => f.Person.Email)
        .RuleFor(x => x.Password, f => password)
        .RuleFor(x => x.Scopes, f => scopes)
        .RuleFor(x => x.IsActive, f => isActive)
        .Generate();
}

public static class Users
{
    public static readonly User AdminUser = new()
    {
        FullName = "Admin",
        Email = "admin@plainly.com",
        Password = "123456"
    };
    public static readonly User TestUser = User.Generate(scopes: [Scopes.Profile]);
    public static readonly User NoScopesUser = User.Generate(scopes: null);
    public static readonly User InActiveUser = User.Generate(isActive: false);
    public static readonly User EditProfileUser = User.Generate(scopes: [Scopes.Profile]);

    public static readonly User[] All =
    [
        TestUser,
        NoScopesUser,
        InActiveUser,
        EditProfileUser
    ];
}