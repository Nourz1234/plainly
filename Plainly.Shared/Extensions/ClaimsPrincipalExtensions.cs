using System.Security.Claims;
using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool CanPerformAction<TAction>(this ClaimsPrincipal claimsPrincipal)
        where TAction : IAction
    {
        var actionScopes = TAction.RequiredScopes.Select(x => x.GetEnumMemberValue()).ToArray();
        if (actionScopes.Length == 0)
        {
            return true;
        }

        // Admin has access to all!
        if (claimsPrincipal.IsInRole(Roles.Admin.Name))
        {
            return true;
        }

        var userScopes = claimsPrincipal.FindAll(JwtClaimNames.Scopes).Select(c => c.Value).ToArray();
        bool userHasScope(string scope) => userScopes.Any(userScope => userScope == scope || scope.StartsWith(userScope + "."));

        return actionScopes.All(userHasScope);
    }
}