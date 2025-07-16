using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Plainly.Api.Exceptions;
using Plainly.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Plainly.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Plainly.Api.Services;
using Plainly.Api.Database;
using Plainly.Api.Infrastructure.ExceptionHandling;
using Plainly.Api.Infrastructure.Action;
using Plainly.Api.Infrastructure.Environment;
using Plainly.Api.Database.Seeders;
using FluentValidation;
using System.ComponentModel;
using System.Reflection;
using Plainly.Api.Infrastructure.AutoValidation;
using Plainly.Shared.Interfaces;
using Plainly.Api.Infrastructure.Logging.Providers;
using Plainly.Api.Infrastructure.Logging.Abstract.Models;

namespace Plainly.Api;

[ExcludeFromCodeCoverage]
public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        // Add controllers
        services.AddControllers()
            .AddAutoValidation() // Add auto validation using FluentValidation
            .AddValidatorsFromAssemblyOf<IAction>(); // Add validators

        // Customize FluentValidation display name resolver
        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
        {
            var displayNameAttribute = memberInfo.GetCustomAttribute<DisplayNameAttribute>()
                ?? throw new InvalidOperationException($"Form field {type.Name}.{memberInfo.Name} is missing {nameof(DisplayNameAttribute)} attribute.");
            return displayNameAttribute.DisplayName;
        };

        // Add OpenAPI
        services.AddOpenApi();

        // Add App DB Context
        services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
        );

        // Add Logging DB Context
        services.AddDbContext<LogDbContext>(
            options => options.UseSqlite(Configuration.GetConnectionString("LoggingConnection")));

        // Add Identity
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });

        // JWT Auth setup
        var rsa = RSA.Create();
        rsa.ImportFromPem(Configuration["Jwt:PublicKey"]);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new RsaSecurityKey(rsa)
                };
            });

        // Add our custom services
        services.AddSingleton<ILoggerProvider, DbLoggerProvider<LogDbContext, LogEntry>>();
        services.AddSingleton(new JwtService(Configuration));
        services.AddActions();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseGlobalExceptionHandling();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseRouting();
        app.MapFallback(context => throw new NotFoundException(Messages.EndpointNotFound));


        if (env.IsDevelopment())
        {
            app.MapOpenApi();

            // Print all endpoints
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();
                Console.WriteLine("Registered Endpoints:");
                foreach (var endpoint in endpointDataSource.Endpoints)
                {
                    if (endpoint is RouteEndpoint routeEndpoint)
                    {
                        Console.WriteLine($"  {routeEndpoint.DisplayName} - {routeEndpoint.RoutePattern.RawText}");
                    }
                }
            });
        }

        if (env.IsTesting())
        {
            AddTestRoutes(app);
        }

        if (env.IsDevelopment() || env.IsTesting())
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (env.IsTesting())
            {
                try
                {
                    dbContext.Database.EnsureDeleted();
                }
                catch { }
                dbContext.Database.EnsureCreated();
            }
            else
            {
                dbContext.Database.Migrate();
            }

            DatabaseSeeder.SeedAllAsync(scope.ServiceProvider).Wait();
        }
    }

    private static void AddTestRoutes(WebApplication app)
    {
        app.MapGet("/exception", (context) => throw new Exception());
        app.MapGet("/internal-error", (context) => throw new InternalServerErrorException());
        app.MapGet("/not-found-error", (context) => throw new NotFoundException());
        app.MapGet("/unauthorized-error", (context) => throw new UnauthorizedException());
        app.MapGet("/forbidden-error", (context) => throw new ForbiddenException());
        app.MapGet("/bad-request-error", (context) => throw new BadRequestException());
    }
}
