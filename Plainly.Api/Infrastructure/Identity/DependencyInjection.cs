namespace Plainly.Api.Infrastructure.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddUserProvider<T>(this IServiceCollection services)
        where T : class
    {
        services.AddHttpContextAccessor();
        services.AddScoped<UserProvider<T>>();
        return services;
    }
}