using Microsoft.AspNetCore.Authorization;
using Plainly.Shared.Interfaces;

namespace Plainly.Frontend.Authorization;

public class AuthorizeActionAttribute<TAction> : AuthorizeAttribute, IAuthorizationRequirement
    where TAction : IAction
{
    public AuthorizeActionAttribute()
    {
        Policy = ActionPolicyRegistry.Register<TAction>();
    }
}
