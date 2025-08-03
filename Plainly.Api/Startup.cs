using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plainly.Api.Actions;
using Plainly.Api.Data.AppDatabase;
using Plainly.Api.Data.AppDatabase.Seeders;
using Plainly.Api.ExceptionHandling;
using Plainly.Api.Exceptions;
using Plainly.Api.Extensions;
using Plainly.Api.Infrastructure.Identity;
using Plainly.Api.Infrastructure.Jwt;
using Plainly.Api.Infrastructure.Logging;
using Plainly.Api.Validation;
using Plainly.Infrastructure.Persistence.AppDatabase.Entities;
using Plainly.Shared;

namespace Plainly.Api;

[ExcludeFromCodeCoverage]
public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        // Add controllers
        services.AddControllers()
            // Add auto validation using FluentValidation
            .AddAutoValidation();

        // Configure FluentValidation
        FluentValidationConfig.Configure();

        // Configure JSON options
        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow;
        });

        // Add OpenAPI
        services.AddOpenApi();

        // Add App DB Context
        services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
        );

        // Add Identity
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            // Should handle password validation with FluentValidation
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 1;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });


        // Add infrastructure
        services.AddGlobalExceptionHandling();
        services.AddJwtAuthentication(Configuration);
        services.AddDbLogging(Configuration);
        services.AddUserProvider<User>();
        services.AddActions();
    }

    public async Task Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();
        app.UseGlobalExceptionHandling();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllers();
        app.MapFallback(context => throw new NotFoundException(Messages.EndpointNotFound));


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
