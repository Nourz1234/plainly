using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Api.Infrastructure.Web;
using Plainly.Shared.Extensions;
using Plainly.Shared.Interfaces;
using Plainly.Shared.Responses;

namespace Plainly.Api.Infrastructure.Authorization;

public class AuthorizeActionFilter<TAction>(TAction action) : IAuthorizationFilter
    where TAction : IAction
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var scopes = action.RequiredScopes;
        if (scopes.Length == 0)
        {
            return;
        }

        var user = context.HttpContext.User;
        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            context.Result = ErrorResponse.Unauthorized().WithTraceId(context.HttpContext.GetTraceId()).Build().ToActionResult();
            return;
        }

        // Admin has access to all!
        if (user.IsInRole(Roles.Admin.Name))
        {
            return;
        }

        var userScopes = user.FindAll("scopes").Select(c => c.Value).ToArray();
        bool hasScope(string scope) => userScopes.Any(userScope => userScope == scope || scope.StartsWith(userScope + "."));

        if (!scopes.Select(x => x.GetEnumMemberValue()).All(hasScope))
        {
            context.Result = ErrorResponse.Forbidden().WithTraceId(context.HttpContext.GetTraceId()).Build().ToActionResult();
            return;
        }
    }
}
