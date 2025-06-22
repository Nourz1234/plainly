using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
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
        }


        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // using static System.Net.Mime.MediaTypeNames;
                context.Response.ContentType = Text.Plain;

                await context.Response.WriteAsync("An exception was thrown.");

                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                {
                    await context.Response.WriteAsync(" The file was not found.");
                }

                if (exceptionHandlerPathFeature?.Path == "/")
                {
                    await context.Response.WriteAsync(" Page: Home.");
                }
            });
        });

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        app.MapFallback(async context =>
        {
            throw new Exception("test");
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
