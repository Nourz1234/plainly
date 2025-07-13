namespace Plainly.Shared.Interfaces;

public interface IActionHandler<TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken token = default);
}

public interface IActionHandler<TActon, TRequest, TResponse> : IActionHandler<TRequest, TResponse>
    where TActon : IAction<TRequest, TResponse>
{ }
