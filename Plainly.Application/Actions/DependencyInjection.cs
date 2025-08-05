using Microsoft.Extensions.DependencyInjection;
using Plainly.Shared.Interfaces;

namespace Plainly.Application.Actions;

public static class DependencyInjection
{
    public static void AddActions(this IServiceCollection services)
    {
        // Add dispatcher
        services.AddScoped<ActionDispatcher>();

        // Add actions and action handlers
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<Plainly.Shared.Main>()
                .AddClasses(c => c.AssignableTo(typeof(IAction<,>)))
                .AsSelf()
                .WithScopedLifetime();
            scan.FromAssemblyOf<Plainly.Application.Main>()
                .AddClasses(c => c.AssignableTo(typeof(IActionHandler<,,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });
    }
}