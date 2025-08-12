using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Plainly.Frontend.Providers;

// public class TokenAuthenticationStateProvider : AuthenticationStateProvider
// {
//     private readonly IJSRuntime js;
//     private ClaimsPrincipal _Anonymous = new(new ClaimsIdentity());

//     public TokenAuthenticationStateProvider(IJSRuntime js)
//     {
//         this.js = js;
//     }

//     public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//     {
//         var token = await js.InvokeAsync<string>("localStorage.getItem", "authToken");

//         if (string.IsNullOrWhiteSpace(token) || IsTokenExpired(token))
//         {
//             return new AuthenticationState(_Anonymous);
//         }

//         var claims = ParseClaimsFromJwt(token);
//         var identity = new ClaimsIdentity(claims, "jwt");
//         return new AuthenticationState(new ClaimsPrincipal(identity));
//     }

//     private bool IsTokenExpired(string token)
//     {
//         var claims = ParseClaimsFromJwt(token);
//         var exp = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
//         if (exp == null) return true;
//         var expTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp));
//         return expTime < DateTimeOffset.UtcNow;
//     }

//     private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
//     {
//         var payload = jwt.Split('.')[1];
//         var jsonBytes = ParseBase64WithoutPadding(payload);
//         var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

//         return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
//     }

//     private byte[] ParseBase64WithoutPadding(string base64)
//     {
//         switch (base64.Length % 4)
//         {
//             case 2: base64 += "=="; break;
//             case 3: base64 += "="; break;
//         }
//         return Convert.FromBase64String(base64);
//     }
// }