using Microsoft.Extensions.DependencyInjection;

namespace Plainly.Infrastructure.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddCurrentUserProvider(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<CurrentUserProvider>();
        return services;
    }
}