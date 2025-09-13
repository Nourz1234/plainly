namespace Plainly.Shared;

public record Role(string Name, Scope[]? Scopes);

public static class Roles
{
    public static readonly Role Admin = new("Admin", null);

    public static readonly Role[] All = [Admin];
}