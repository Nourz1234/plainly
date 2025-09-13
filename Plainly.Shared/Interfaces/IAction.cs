namespace Plainly.Shared.Interfaces;

public interface IAction
{
    public static abstract ActionId Id { get; }
    public static abstract string DisplayName { get; }
    public static abstract Scope[] RequiredScopes { get; }
}

public interface IAction<TRequest> : IAction
{ }

public interface IAction<TRequest, TResponse> : IAction<TRequest>
{ }
