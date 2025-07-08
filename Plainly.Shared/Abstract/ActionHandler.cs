using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Abstract;

// public abstract class ActionHandler<TAction, TRequest, TResponse>(TAction action) : IActionHandler<TAction, TRequest, TResponse>
//     where TAction : IAction<TRequest, TResponse>
//     where TRequest : class
//     where TResponse : class
// {
//     public TAction Action { get; } = action;

//     public abstract Task<TResponse> Handle(TRequest request);
// }