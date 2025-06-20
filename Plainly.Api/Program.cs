using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapFallback(async context =>
{
    context.Response.StatusCode = 404;
    context.Response.ContentType = "application/json";

    var result = new
    {
        status = 404,
        error = "Endpoint not found",
        path = context.Request.Path,
        method = context.Request.Method
    };

    var json = JsonSerializer.Serialize(result);
    await context.Response.WriteAsync(json);
});



var appRunTask = app.RunAsync();

if (app.Environment.IsDevelopment())
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
}

await appRunTask;