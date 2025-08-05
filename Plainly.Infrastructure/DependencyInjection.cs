using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plainly.Infrastructure.Identity;
using Plainly.Infrastructure.Jwt;
using Plainly.Infrastructure.Logging;
using Plainly.Infrastructure.Persistence;

namespace Plainly.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddIdentity();
        services.AddDbLogging(configuration);
        services.AddJwtAuthentication(configuration);

        return services;
    }
}