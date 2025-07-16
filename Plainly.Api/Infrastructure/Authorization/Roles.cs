using Plainly.Shared;

namespace Plainly.Api.Infrastructure.Authorization;

public record Role(string Name, Scopes[]? Scopes);

public static class Roles
{
    public static readonly Role Admin = new("Admin", null);

    public static readonly Role[] All = [Admin];
}