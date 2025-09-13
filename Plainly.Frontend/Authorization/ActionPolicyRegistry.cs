using Plainly.Shared.Interfaces;

namespace Plainly.Frontend.Authorization;

public static class ActionPolicyRegistry
{
    private static readonly Dictionary<string, Func<ActionRequirement>> _RequirementsRegistry = [];
    public static string Register<TAction>()
        where TAction : IAction
    {
        var policy = $"action:{TAction.Id}";
        if (!_RequirementsRegistry.ContainsKey(policy))
        {
            _RequirementsRegistry.Add(policy, () => new ActionRequirement<TAction>());
        }
        return policy;
    }

    public static bool HasPolicy(string policyName) => _RequirementsRegistry.ContainsKey(policyName);
    public static ActionRequirement GetRequirement(string policyName) => _RequirementsRegistry[policyName]();
}