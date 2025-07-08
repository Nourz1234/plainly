namespace Plainly.Shared.Interfaces;

public interface IAction
{
    public string DisplayName { get; }
    public string InternalName { get; }
    public string? Claim { get; }
}

public interface IAction<TRequest> : IAction
{ }