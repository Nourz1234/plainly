using Plainly.Shared.Interfaces;

namespace Plainly.Frontend.Authorization;

public static class RegisteredActions
{
    private static Dictionary<string, Type> Actions { get; } = [];

    public static string Add<TAction>() where TAction : IAction
    {
        var actionName = nameof(TAction);
        Actions.Add(actionName, typeof(TAction));
        return actionName;
    }

    public static IAction Get(string actionName) => Activator.CreateInstance(Actions[actionName]) as IAction ?? throw new Exception("Action not found");

    public static bool Has(string actionName) => Actions.ContainsKey(actionName);
}