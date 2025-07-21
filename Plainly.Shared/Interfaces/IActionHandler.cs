namespace Plainly.Shared.Interfaces;

public interface IActionHandler<TActon, TRequest>
    where TActon : IAction<TRequest>
{
    public Task Handle(TRequest request, CancellationToken token = default);
}

public interface IActionHandler<TActon, TRequest, TResponse>
    where TActon : IAction<TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken token = default);
}
