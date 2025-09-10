using Microsoft.AspNetCore.Authorization;
using Plainly.Shared.Interfaces;

namespace Plainly.Frontend.Authorization;

public class AuthorizeActionAttribute<TAction> : AuthorizeAttribute
    where TAction : IAction
{
    public AuthorizeActionAttribute()
    {
        Policy = RegisteredActions.Add<TAction>();
    }
}
