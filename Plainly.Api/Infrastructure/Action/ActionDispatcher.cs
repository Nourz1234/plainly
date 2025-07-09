using Plainly.Api.Exceptions;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Action;

public class ActionDispatcher(IServiceProvider _Provider)
{
    public Task<TResponse> Dispatch<TResponse>(IActionRequest<TResponse> request)
    {
        var action = _Provider.GetService<IAction<IActionRequest<TResponse>>>() ?? throw new NotFoundException("The requested action was not found.");
        var handler = ResolveHandler<IActionRequest<TResponse>, TResponse>();
        return handler.Handle(request);
    }

    private IActionHandler<IAction<TRequest>, TRequest, TResponse> ResolveHandler<TRequest, TResponse>()
        where TRequest : IActionRequest<TResponse>
    {
        return _Provider.GetService<IActionHandler<IAction<TRequest>, TRequest, TResponse>>()
            ?? throw new NotFoundException("The requested action was not found.");
    }
}