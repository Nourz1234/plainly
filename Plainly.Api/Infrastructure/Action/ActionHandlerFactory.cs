using Plainly.Api.Exceptions;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Action;

public class ActionHandlerFactory(IServiceProvider services)
{
    private readonly IServiceProvider _Services = services;

    public IActionHandler<TAction, TRequest, TResponse> GetHandler<TAction, TRequest, TResponse>()
        where TAction : IAction<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        return _Services.GetService<IActionHandler<TAction, TRequest, TResponse>>()
            ?? throw new NotFoundException("The requested action was not found.");
    }
}