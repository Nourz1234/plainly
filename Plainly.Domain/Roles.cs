using Plainly.Shared;

namespace Plainly.Domain;

public record Role(string Name, Scopes[]? Scopes);

public static class Roles
{
    public static readonly Role Admin = new("Admin", null);

    public static readonly Role[] All = [Admin];
}