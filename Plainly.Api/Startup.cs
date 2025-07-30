using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using Plainly.Api.Data.AppDatabase;
using Plainly.Api.Data.AppDatabase.Seeders;
using Plainly.Api.Exceptions;
using Plainly.Api.Infrastructure.AutoValidation;
using Plainly.Api.Infrastructure.Environment;
using Plainly.Api.Infrastructure.ExceptionHandling;
using Plainly.Api.Infrastructure.Jwt;
using Plainly.Shared;
using Plainly.Shared.Interfaces;
using Plainly.Api.Infrastructure.Actions;
using Plainly.Api.Entities;
using System.Diagnostics;
using Plainly.Api.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc;
using Plainly.Shared.Responses;
using Plainly.Api.Infrastructure.Web;
using Plainly.Api.Infrastructure.Identity;
using System.Text.Json;

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

        // Add JSON options
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

        // JWT Auth setup
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(Configuration["Jwt:PublicKey"]);
                var parameter = rsa.ExportParameters(false);
                // JsonSerializer.Deserialize(parameter)
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
        services.AddDbLogging(Configuration);
        services.AddJwtService(Configuration);
        services.AddUserProvider<User>();
        services.AddActions();

        // Customize 400 response
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                // TODO: Add logging
                var extraFields = context.ModelState
                    .Where(x => x.Key.StartsWith("$."))
                    .Select(x => x.Key.Replace("$.", ""))
                    .ToArray();
                if (extraFields.Length > 0)
                {
                    var errors = new Dictionary<string, ValidationErrorDetail[]>
                    {
                        [""] = extraFields.Select(
                            field => new ValidationErrorDetail(string.Format(Messages.UnknownField, field), ErrorCode.UnknownField.ToString())
                        ).ToArray()
                    };
                    return new ValidationErrorResponse
                    {
                        Message = Messages.ValidationError,
                        Errors = errors,
                        TraceId = context.HttpContext.GetTraceId()
                    }.Convert();
                }

                return new ErrorResponse(StatusCodes.Status400BadRequest)
                {
                    Message = Messages.BadRequest,
                    TraceId = context.HttpContext.GetTraceId()
                }.Convert();
            };
        });
    }

    public async Task Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseRouting();

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
        // Add test routes
        app.MapGet("/exception", (context) => throw new Exception());
        app.MapGet("/internal-error", (context) => throw new InternalServerErrorException());
        app.MapGet("/not-found-error", (context) => throw new NotFoundException());
        app.MapGet("/unauthorized-error", (context) => throw new UnauthorizedException());
        app.MapGet("/forbidden-error", (context) => throw new ForbiddenException());
        app.MapGet("/bad-request-error", (context) => throw new BadRequestException());


        // Reset database
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try { dbContext.Database.EnsureDeleted(); }
        catch { }
        dbContext.Database.EnsureCreated();
        await DatabaseSeeder.SeedAllAsync(scope.ServiceProvider);
    }
}
