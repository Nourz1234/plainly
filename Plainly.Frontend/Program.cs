using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Plainly.Frontend.Providers;
using Plainly.Frontend.Services;

namespace Plainly.Frontend;

public class Program
{
    private static async Task Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddHttpClient<ApiService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? throw new Exception("ApiBaseUrl is not set"));
        });
        builder.Services.AddScoped<CurrentUserService>();
        builder.Services.AddScoped<JwtTokenValidationService>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

        await builder.Build().RunAsync();
    }
}