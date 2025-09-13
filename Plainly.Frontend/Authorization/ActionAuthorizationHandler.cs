using Microsoft.AspNetCore.Authorization;

namespace Plainly.Frontend.Authorization;

public class ActionAuthorizationHandler : AuthorizationHandler<ActionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionRequirement requirement)
    {
        if (requirement.UserCanPerformAction(context.User))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
