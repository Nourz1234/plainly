using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions;

public class ActionDispatcher(IServiceProvider serviceProvider)
{
    public Task Dispatch<TAction, TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TAction : IAction<TRequest>
    {
        // TODO: Add logging
        var handler = serviceProvider.GetRequiredService<IActionHandler<TAction, TRequest>>();
        return handler.Handle(request, cancellationToken);
    }

    public Task<TResponse> Dispatch<TAction, TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TAction : IAction<TRequest, TResponse>
    {
        // TODO: Add logging
        var handler = serviceProvider.GetRequiredService<IActionHandler<TAction, TRequest, TResponse>>();
        return handler.Handle(request, cancellationToken);
    }
}