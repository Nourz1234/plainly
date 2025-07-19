using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Actions;

public static class DependencyInjection
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