using System.Net.Http.Json;
using Plainly.Shared.Actions.Auth.Login;
using Plainly.Shared.Actions.Heath.GetHealth;
using Plainly.Shared.Responses;

namespace Plainly.Frontend.Services;

public class ApiService(HttpClient httpClient)
{
    private readonly HttpClient _HttpClient = httpClient;

    public async Task<GetHealthDTO> GetHealthAsync()
    {
        var result = await _HttpClient.GetFromJsonAsync<SuccessResponse<GetHealthDTO>>("api/Health");
        return result!.Data;
    }

    public async Task<LoginDTO> LoginAsync(LoginForm form)
    {
        var response = await _HttpClient.PostAsJsonAsync("api/Auth/Login", form);
        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<LoginDTO>>();
        return result!.Data;
    }
}