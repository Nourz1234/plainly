using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Plainly.Infrastructure.Persistence.AppDatabase;
using Plainly.Infrastructure.Persistence.AppDatabase.Seeders;
using Plainly.Infrastructure.Persistence.LogDatabase;

namespace Plainly.Api;

[ExcludeFromCodeCoverage]
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var startup = new Startup(builder.Configuration);

        startup.ConfigureServices(builder.Services);

        var app = builder.Build();

        await startup.Configure(app, builder.Environment);

        if (args.Contains("--migrate"))
        {
            using var scope = app.Services.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            appDbContext.Database.Migrate();
            var logDbContext = scope.ServiceProvider.GetRequiredService<LogDbContext>();
            logDbContext.Database.Migrate();
        }
        else if (args.Contains("--seed"))
        {
            using var scope = app.Services.CreateScope();
            await DatabaseSeeder.SeedAllAsync(scope.ServiceProvider);
        }
        else
        {
            await app.RunAsync();
        }
    }
}
