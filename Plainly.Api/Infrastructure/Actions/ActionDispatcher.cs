using Plainly.Api.Exceptions;
using Plainly.Shared;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Actions;

public class ActionDispatcher(IServiceProvider serviceProvider)
{
    public Task<TResponse> Dispatch<TAction, TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TAction : IAction<TRequest, TResponse>
    {
        var handler = serviceProvider.GetService<IActionHandler<TAction, TRequest, TResponse>>()
            ?? throw new NotFoundException(Messages.EndpointNotFound);
        return handler.Handle(request, cancellationToken);
    }
}