namespace Plainly.Shared.Interfaces;
public interface IActionHandler<TAction, TRequest, TResponse>
    where TAction : IAction<TRequest>
    where TRequest : IActionRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request);
}
