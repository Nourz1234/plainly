using Plainly.Api.Exceptions;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Action.Services;

public class ActionDispatcher(IServiceProvider serviceProvider)
{
    public Task<TResponse> Dispatch<TAction, TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TAction : IAction<TRequest, TResponse>
    {
        var handler = serviceProvider.GetService<IActionHandler<TAction, TRequest, TResponse>>()
            ?? throw new NotFoundException("The requested action was not found.");
        return handler.Handle(request, cancellationToken);
    }
}