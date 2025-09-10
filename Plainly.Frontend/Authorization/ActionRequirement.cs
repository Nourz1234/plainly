using Microsoft.AspNetCore.Authorization;
using Plainly.Shared.Interfaces;

namespace Plainly.Frontend.Authorization;

public class ActionRequirement(IAction action) : IAuthorizationRequirement
{
    public IAction Action { get; } = action;
}
