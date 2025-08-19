using Plainly.Frontend.Services;

namespace Plainly.Frontend.Handlers;

public class JwtAuthorizationMessageHandler(CurrentUserService currentUserService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (currentUserService.Token is not null)
        {
            request.Headers.Authorization = new("Bearer", currentUserService.Token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}