using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plainly.Domain.Entities;
using Plainly.Infrastructure.Logging.Providers;
using Plainly.Infrastructure.Persistence.LogDatabase;

namespace Plainly.Infrastructure.Logging;


public static class DependencyInjection
{
    public static IServiceCollection AddDbLogging(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["DbLogging:Enabled"] != "true") return services;

        // Add logger provider
        services.AddSingleton<ILoggerProvider, DbLoggerProvider<LogDbContext, LogEntry>>();

        return services;
    }
}