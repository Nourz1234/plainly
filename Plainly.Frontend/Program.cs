using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Plainly.Frontend;
using Plainly.Frontend.Providers;
using Plainly.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var configJson = await http.GetStringAsync("appsettings.json");
var config = JsonSerializer.Deserialize<AppConfig>(configJson)!;

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddSingleton(config);
builder.Services.AddSingleton<JwtValidator>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(config.ApiBaseUrl) });

// Add authentication
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
builder.Services.AddScoped<TokenAuthenticationStateProvider>();

await builder.Build().RunAsync();
