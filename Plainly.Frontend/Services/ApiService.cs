using System.Net.Http.Json;
using Plainly.Shared.Actions.Auth.Login;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Actions.Heath.GetHealth;
using Plainly.Shared.Actions.User.EditProfile;
using Plainly.Shared.Actions.User.ViewProfile;
using Plainly.Shared.Responses;

namespace Plainly.Frontend.Services;

public class ApiService
{
    private readonly HttpClient _HttpClient;

    public ApiService(IConfiguration configuration, ApiMessageHandler apiMessageHandler)
    {
        _HttpClient = new(apiMessageHandler)
        {
            BaseAddress = new Uri(configuration["ApiBaseUrl"] ?? throw new Exception("ApiBaseUrl is not set"))
        };
    }

    public async Task<SuccessResponse<GetHealthDTO>> GetHealthAsync()
    {
        var result = await _HttpClient.GetFromJsonAsync<SuccessResponse<GetHealthDTO>>("api/Health");
        return result!;
    }

    public async Task<SuccessResponse<ViewProfileDTO>> ViewProfileAsync()
    {
        var result = await _HttpClient.GetFromJsonAsync<SuccessResponse<ViewProfileDTO>>("api/User/Profile");
        return result!;
    }

    public async Task<SuccessResponse<EditProfileDTO>> EditProfileAsync(EditProfileFrom form)
    {
        var response = await _HttpClient.PatchAsJsonAsync("api/User/Profile", form);
        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<EditProfileDTO>>();
        return result!;
    }

    public async Task<SuccessResponse<LoginDTO>> LoginAsync(LoginForm form)
    {
        var response = await _HttpClient.PostAsJsonAsync("api/Auth/Login", form);
        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<LoginDTO>>();
        return result!;
    }

    public async Task<SuccessResponse<RegisterDTO>> RegisterAsync(RegisterForm form)
    {
        var response = await _HttpClient.PostAsJsonAsync("api/Auth/Register", form);
        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<RegisterDTO>>();
        return result!;
    }
}