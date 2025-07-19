using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Api.Exceptions;
using Plainly.Api.Infrastructure.Web;
using Plainly.Shared;
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
            context.Result = new ErrorResponse(StatusCodes.Status401Unauthorized)
            {
                Message = Messages.Unauthorized,
                TraceId = context.HttpContext.GetTraceId()
            }.Convert();
            return;
        }

        // Admin has access to all!
        if (user.IsInRole(Roles.Admin.Name))
        {
            return;
        }

        if (!scopes.All(scope => user.HasClaim("scopes", scope.GetEnumMemberValue())))
        {
            context.Result = new ErrorResponse(StatusCodes.Status403Forbidden)
            {
                Message = Messages.Forbidden,
                TraceId = context.HttpContext.GetTraceId()
            }.Convert();
        }
    }
}
