using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Api.Builders;
using Plainly.Api.Extensions;
using Plainly.Domain;
using Plainly.Shared.Extensions;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Authorization;

public class AuthorizeActionFilter<TAction> : IAuthorizationFilter
    where TAction : IAction
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var actionScopes = TAction.RequiredScopes.Select(x => x.GetEnumMemberValue()).ToArray();
        if (actionScopes.Length == 0)
        {
            return;
        }

        var user = context.HttpContext.User;
        if (user.Identity is not { IsAuthenticated: true })
        {
            context.Result = ErrorResponseBuilder.FromErrorCode(DomainErrorCode.Unauthorized).Build(context.HttpContext).ToActionResult();
            return;
        }

        if (!user.CanPerformAction<TAction>())
        {
            context.Result = ErrorResponseBuilder.FromErrorCode(DomainErrorCode.Forbidden).Build(context.HttpContext).ToActionResult();
            return;
        }
    }
}
