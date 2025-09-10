using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Plainly.Frontend.Authorization;

public class ActionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (RegisteredActions.Has(policyName))
        {
            var action = RegisteredActions.Get(policyName);

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new ActionRequirement(action))
                .Build();

            return policy;
        }

        return await base.GetPolicyAsync(policyName);
    }
}
