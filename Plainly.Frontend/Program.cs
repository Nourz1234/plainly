using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Plainly.Frontend.Authorization;
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
        builder.Services.AddBlazoredLocalStorageAsSingleton();
        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.HideTransitionDuration = 150;
            config.SnackbarConfiguration.ShowTransitionDuration = 150;
            config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomRight;
        });
        builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
        builder.Services.AddScoped<IAuthorizationHandler, ActionAuthorizationHandler>();
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, ActionAuthorizationPolicyProvider>();

        // register all services as singleton since this is wasm
        builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddSingleton<LocalStorageWatcher>();
        builder.Services.AddSingleton<CurrentUserService>();
        builder.Services.AddSingleton<JwtTokenValidationService>();
        builder.Services.AddSingleton<ApiMessageHandler>();
        builder.Services.AddSingleton<ApiService>();

        await builder.Build().RunAsync();
    }
}