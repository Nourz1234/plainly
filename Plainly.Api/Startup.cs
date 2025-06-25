using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Plainly.Api.Exceptions;
using Plainly.Api.Middleware;
using static System.Net.Mime.MediaTypeNames;

namespace Plainly.Api;

[ExcludeFromCodeCoverage]
public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapGet("/exception", (context) => throw new Exception());
            app.MapGet("/internal-error", (context) => throw new InternalServerErrorException());
            app.MapGet("/not-found-error", (context) => throw new NotFoundException());
            app.MapGet("/unauthorized-error", (context) => throw new UnauthorizedException());
            app.MapGet("/forbidden-error", (context) => throw new ForbiddenException());
            app.MapGet("/bad-request-error", (context) => throw new BadRequestException());
            app.MapGet("/validation-error", (context) => throw new ValidationException());
            app.MapGet("/validation-error-with-errors", (context) => throw new ValidationException(new Dictionary<string, string[]> { ["field1"] = ["test"] }));
        }


        app.UseGlobalExceptionHandling();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();


        app.MapFallback(async context =>
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";

            var result = new
            {
                status = 404,
                error = "Endpoint not found!",
                path = context.Request.Path,
                method = context.Request.Method
            };

            var json = JsonSerializer.Serialize(result);
            await context.Response.WriteAsync(json);
        });

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
}
