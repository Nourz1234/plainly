using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Api.Exceptions;
using Plainly.Shared.Extensions;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Authorization.Filters;

public class AuthorizeForFilter<TAction> : IAuthorizationFilter
    where TAction : IAction
{
    private readonly TAction _Action;
    private readonly IHttpContextAccessor _HttpContextAccessor;

    public AuthorizeForFilter(TAction action, IHttpContextAccessor httpContextAccessor)
    {
        _Action = action;
        _HttpContextAccessor = httpContextAccessor;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var scopes = _Action.RequiredScopes;
        if (scopes.Length == 0)
        {
            return;
        }

        var user = _HttpContextAccessor.HttpContext?.User;
        if (user is null)
        {
            context.Result = new UnauthorizedException().ToActionResult();
            return;
        }

        if (user.IsInRole(Roles.Admin.Name))
        {
            return;
        }
        if (!scopes.All(scope => user.HasClaim("scopes", scope.GetEnumMemberValue())))
        {
            context.Result = new ForbiddenException().ToActionResult();
        }
    }
}
