using System.Diagnostics.CodeAnalysis;
using Plainly.Api.Data.AppDatabase.Seeders;

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

        if (args.Contains("--seed"))
        {
            await DatabaseSeeder.SeedAllAsync(app.Services);
        }
        else
        {
            await app.RunAsync();
        }
    }
}
