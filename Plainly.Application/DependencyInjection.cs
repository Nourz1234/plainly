using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plainly.Application.Actions;

namespace Plainly.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Add actions
        services.AddActions();

        return services;
    }
}