using Plainly.Api.Infrastructure.Action.Services;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Action;

public static class ActionExtensions
{
    public static void AddActions(this IServiceCollection services)
    {
        // Add dispatcher
        services.AddScoped<ActionDispatcher>();

        // Add actions and action handlers
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<IAction>()
                .AddClasses(c => c.AssignableTo(typeof(IAction<,>)))
                .AsSelf()
                .WithScopedLifetime();
            scan.FromAssemblyOf<Program>()
                .AddClasses(c => c.AssignableTo(typeof(IActionHandler<,,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });
    }
}