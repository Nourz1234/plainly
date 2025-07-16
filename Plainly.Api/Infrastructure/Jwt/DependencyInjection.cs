namespace Plainly.Api.Infrastructure.Jwt;

public static class DependencyInjection
{
    public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        // Add options
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        // Add services
        services.AddScoped<JwtService>();
        return services;
    }
}