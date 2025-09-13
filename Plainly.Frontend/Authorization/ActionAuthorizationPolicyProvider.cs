using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Plainly.Frontend.Authorization;

public class ActionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (ActionPolicyRegistry.HasPolicy(policyName))
        {
            var requirement = ActionPolicyRegistry.GetRequirement(policyName);

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(requirement)
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return base.GetPolicyAsync(policyName);
    }
}
