using Microsoft.AspNetCore.Authorization;
using Plainly.Api.Actions.Auth;

namespace Plainly.Api.Actions;

public static class Features
{
    private static readonly Dictionary<string, Type> AllActions = new(){
        { RegisterAction.Scope, typeof(RegisterAction) }
    };

    public static void AddActionsPolicies(this AuthorizationOptions options)
    {
        foreach (string scope in AllActions.Keys)
            options.AddPolicy(scope, policy => policy.RequireClaim(scope));
    }
}