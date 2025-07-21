namespace Plainly.Shared.Interfaces;

public interface IAction
{
    public string DisplayName { get; }
    public string InternalName { get; }
    public Scopes[] RequiredScopes { get; }
}

public interface IAction<TRequest> : IAction
{ }

public interface IAction<TRequest, TResponse> : IAction
{ }
