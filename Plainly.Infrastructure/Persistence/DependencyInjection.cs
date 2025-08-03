using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plainly.Domain.Interfaces.Repositories;
using Plainly.Infrastructure.Persistence.AppDatabase;
using Plainly.Infrastructure.Persistence.AppDatabase.Repositories;
using Plainly.Infrastructure.Persistence.LogDatabase;

namespace Plainly.Infrastructure.Persistence;


public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Add App DB Context
        services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );
        // Add Logging DB Context
        services.AddDbContext<LogDbContext>(
            options => options.UseSqlite(configuration["DbLogging:ConnectionString"]));

        // Add repositories
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}