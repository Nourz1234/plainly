using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Api.Exceptions;
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
        var claim = _Action.Claim;
        if (claim is null)
        {
            return;
        }

        // var user = _HttpContextAccessor.HttpContext?.User;
        // if (user is null)
        // {
        //     context.Result = new UnauthorizedException().ToActionResult();
        //     return;
        // }

        // if (user.IsInRole("admin"))
        // {
        //     return;
        // }
        // if (!user.HasClaim("permission", claim))
        // {
        //     context.Result = new ForbiddenException().ToActionResult();
        // }
    }
}
