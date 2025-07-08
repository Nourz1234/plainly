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
using Plainly.Api.Interfaces;
using Plainly.Api.Infrastructure.ExceptionHandling;
using Plainly.Api.Infrastructure.Action;
using Plainly.Shared.Interfaces;

namespace Plainly.Api;

[ExcludeFromCodeCoverage]
public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        // Configure EF Core
        services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
        );

        // Add Identity
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

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
        services.AddSingleton<IAuthTokenService>(new JwtService(Configuration));

        services.AddHttpContextAccessor();
        services.AddActionFactory();

        services.Scan(scan =>
        {
            scan.FromAssemblyOf<IAction>()
            .AddClasses(classes => classes.AssignableTo(typeof(IAction<>)))
            ;
        });
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseGlobalExceptionHandling();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseRouting();

        app.MapFallback(context =>
        {
            throw new NotFoundException(Messages.EndpointNotFound);
        });

        if (env.IsEnvironment("Testing"))
        {
            AddTestRoutes(app);
        }

        if (app.Environment.IsDevelopment())
        {
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
    }

    private static void AddTestRoutes(WebApplication app)
    {
        app.MapGet("/exception", (context) => throw new Exception());
        app.MapGet("/internal-error", (context) => throw new InternalServerErrorException());
        app.MapGet("/not-found-error", (context) => throw new NotFoundException());
        app.MapGet("/unauthorized-error", (context) => throw new UnauthorizedException());
        app.MapGet("/forbidden-error", (context) => throw new ForbiddenException());
        app.MapGet("/bad-request-error", (context) => throw new BadRequestException());
        app.MapGet("/validation-error", (context) => throw new ValidationException());
        app.MapGet("/validation-error-with-errors", (context) => throw new ValidationException(new Dictionary<string, string[]> { ["field1"] = ["test"] }));
    }
}
