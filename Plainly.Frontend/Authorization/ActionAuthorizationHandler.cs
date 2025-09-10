using Microsoft.AspNetCore.Authorization;
using Plainly.Shared.Extensions;
using Plainly.Frontend.Services;

namespace Plainly.Frontend.Authorization;

public class ActionAuthorizationHandler(CurrentUserService currentUserService) : AuthorizationHandler<ActionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionRequirement requirement)
    {
        var action = requirement.Action;
        if (currentUserService.CanPerformAction(action))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
