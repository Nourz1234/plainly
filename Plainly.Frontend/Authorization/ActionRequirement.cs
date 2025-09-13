using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Plainly.Shared.Extensions;
using Plainly.Shared.Interfaces;

namespace Plainly.Frontend.Authorization;

public abstract class ActionRequirement : IAuthorizationRequirement
{
    public abstract bool UserCanPerformAction(ClaimsPrincipal user);
}

public class ActionRequirement<TAction> : ActionRequirement
    where TAction : IAction
{
    public override bool UserCanPerformAction(ClaimsPrincipal user) => user.CanPerformAction<TAction>();
}

