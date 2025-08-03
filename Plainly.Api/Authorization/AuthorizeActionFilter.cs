using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Api.Extensions;
using Plainly.Shared.Interfaces;
using Plainly.Shared.Responses;

namespace Plainly.Api.Authorization;

public class AuthorizeActionFilter<TAction>(TAction action) : IAuthorizationFilter
    where TAction : IAction
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var actionScopes = action.RequiredScopes.Select(x => x.GetEnumMemberValue()).ToArray();
        if (actionScopes.Length == 0)
        {
            return;
        }

        var user = context.HttpContext.User;
        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            context.Result = ErrorResponse.Unauthorized().Build(context.HttpContext.GetTraceId()).ToActionResult();
            return;
        }

        // Admin has access to all!
        if (user.IsInRole(Roles.Admin.Name))
        {
            return;
        }

        var userScopes = user.FindAll("scopes").Select(c => c.Value).ToArray();
        bool userHasScope(string scope) => userScopes.Any(userScope => userScope == scope || scope.StartsWith(userScope + "."));

        if (!actionScopes.All(userHasScope))
        {
            context.Result = ErrorResponse.Forbidden().Build(context.HttpContext.GetTraceId()).ToActionResult();
            return;
        }
    }
}
