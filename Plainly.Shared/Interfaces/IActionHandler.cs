namespace Plainly.Shared.Interfaces;

public interface IActionHandler<TAction, TRequest, TResponse>
    where TAction : IAction<TRequest, TResponse>
    where TRequest : class
    where TResponse : class
{
    public Task<TResponse> Handle(TRequest request);
}
