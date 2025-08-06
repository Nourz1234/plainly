using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plainly.Api.ExceptionHandling;
using Plainly.Api.Exceptions;
using Plainly.Api.Extensions;
using Plainly.Api.Filters;
using Plainly.Api.Validation;
using Plainly.Application;
using Plainly.Infrastructure;
using Plainly.Infrastructure.Persistence.AppDatabase;
using Plainly.Infrastructure.Persistence.AppDatabase.Seeders;

namespace Plainly.Api;

[ExcludeFromCodeCoverage]
public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        // Add controllers
        services.AddControllers(options =>
        {
            options.Filters.Add<ResponseResultFilter>();
        })
            .AddAutoValidation(); // Add auto validation using FluentValidation

        // Configure FluentValidation
        FluentValidationConfig.Configure();

        // Configure JSON options
        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow;
        });

        // Add OpenAPI
        services.AddOpenApi();


        services.AddInfrastructure(Configuration);
        services.AddApplication(Configuration);

        // Add infrastructure
        services.AddGlobalExceptionHandling();
    }

    public async Task Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseHttpsRedirection();
        app.UseGlobalExceptionHandling();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllers();
        app.MapFallback(context => throw new ApiException(ApiErrorCode.EndpointNotFound));


        if (env.IsDevelopment())
        {
            await ConfigureDevelopment(app);
        }
        else if (env.IsTesting())
        {
            await ConfigureTesting(app);
        }
    }

    protected async Task ConfigureDevelopment(WebApplication app)
    {
        app.MapOpenApi();

        // Print all endpoints
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();
            Debug.WriteLine("Registered Endpoints:");
            foreach (var endpoint in endpointDataSource.Endpoints)
            {
                if (endpoint is RouteEndpoint routeEndpoint)
                {
                    Debug.WriteLine($"  {routeEndpoint.DisplayName} - {routeEndpoint.RoutePattern.RawText}");
                }
            }
        });

        // Run migrations and seeders
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();
        await DatabaseSeeder.SeedAllAsync(scope.ServiceProvider);
    }

    protected async Task ConfigureTesting(WebApplication app)
    {
        // Reset database
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try { dbContext.Database.EnsureDeleted(); }
        catch { }
        dbContext.Database.Migrate();
        await DatabaseSeeder.SeedAllAsync(scope.ServiceProvider);
    }
}
