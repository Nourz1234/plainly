using System.Net.Http.Json;
using Plainly.Frontend.Errors;
using Plainly.Shared.Responses;

namespace Plainly.Frontend.Services;

public class ApiMessageHandler(CurrentUserService currentUserService) : DelegatingHandler(new HttpClientHandler())
{
    private readonly CurrentUserService _CurrentUserService = currentUserService;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_CurrentUserService.Token is not null)
        {
            request.Headers.Authorization = new("Bearer", _CurrentUserService.Token);
        }

        var response = await base.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken) ?? throw new ApiError(Messages.InvalidApiResponse);
            throw new ApiError(error.Message)
            {
                Errors = error.Errors,
                TraceId = error.TraceId
            };
        }

        return response;
    }
}

