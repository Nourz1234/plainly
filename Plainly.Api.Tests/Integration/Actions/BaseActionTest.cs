using System.Net.Http.Json;
using Plainly.Api.Tests.Integration.Data;
using Plainly.Shared.Interfaces;
using Plainly.Shared.Responses;

namespace Plainly.Api.Tests.Integration.Actions;

[Collection("App collection")]
public abstract class BaseActionTest(AppFixture appFixture)
{
    protected readonly AppFixture _AppFixture = appFixture;

    protected abstract HttpMethod Method { get; }
    protected abstract string Endpoint { get; }

    protected async Task<HttpResponseMessage> PerformActionAsync(object[]? routeParameters, HttpContent? content, User? asUser, CancellationToken cancellationToken = default)
    {
        var client = asUser is null ? _AppFixture.Client : await _AppFixture.GetClientForUser(asUser);
        var endpoint = routeParameters is null ? Endpoint : string.Format(Endpoint, routeParameters);
        var requestMessage = new HttpRequestMessage(Method, endpoint)
        {
            Content = content
        };
        return await client.SendAsync(requestMessage, cancellationToken);
    }

    protected Task<HttpResponseMessage> PerformActionAsync(object[] routeParameters, HttpContent content, CancellationToken cancellationToken = default)
    {
        return PerformActionAsync(routeParameters, content, null, cancellationToken);
    }

    protected Task<HttpResponseMessage> PerformActionAsync(HttpContent content, CancellationToken cancellationToken = default)
    {
        return PerformActionAsync(null, content, null, cancellationToken);
    }

    protected Task<HttpResponseMessage> PerformActionAsync(CancellationToken cancellationToken = default)
    {
        return PerformActionAsync(null, null, null, cancellationToken);
    }

    protected Task<HttpResponseMessage> PerformActionAsync(User asUser, CancellationToken cancellationToken = default)
    {
        return PerformActionAsync(null, null, asUser, cancellationToken);
    }

    protected Task<HttpResponseMessage> PerformActionAsync(User asUser, HttpContent content, CancellationToken cancellationToken = default)
    {
        return PerformActionAsync(null, content, asUser, cancellationToken);
    }
}

public abstract class BaseActionTest<TAction, TRequest>(AppFixture appFixture) : BaseActionTest(appFixture)
    where TAction : IAction<TRequest>
{
    // protected abstract TRequest[] GetValidRequests();
    // protected abstract TRequest[] GetInvalidRequests();
}


public abstract class BaseActionTest<TAction, TRequest, TResponse>(AppFixture appFixture) : BaseActionTest<TAction, TRequest>(appFixture)
    where TAction : IAction<TRequest, TResponse>
{
    protected Task<SuccessResponse<TResponse>?> GetResultAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        return response.Content.ReadFromJsonAsync<SuccessResponse<TResponse>>(cancellationToken);
    }

    protected Task<ErrorResponse?> GetErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        return response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken);
    }

    protected Task<ValidationErrorResponse?> GetValidationErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        return response.Content.ReadFromJsonAsync<ValidationErrorResponse>(cancellationToken);
    }
}

