using Microsoft.EntityFrameworkCore;
using Plainly.Api.Data.LogDatabase;
using Plainly.Api.Entities;
using Plainly.Api.Infrastructure.Logging.Providers;

namespace Plainly.Api.Infrastructure.Logging;


public static class DependencyInjection
{
    public static IServiceCollection AddDbLogging(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["DbLogging:Enabled"] != "true") return services;

        // Add Logging DB Context
        services.AddDbContext<LogDbContext>(
            options => options.UseSqlite(configuration["DbLogging:ConnectionString"]));

        // Add logger provider
        services.AddSingleton<ILoggerProvider, DbLoggerProvider<LogDbContext, LogEntry>>();

        return services;
    }
}