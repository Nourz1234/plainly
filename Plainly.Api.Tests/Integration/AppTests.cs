using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Plainly.Domain;

namespace Plainly.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
[Collection("App collection")]
public class AppTests(AppFixture appFixture)
{
    [Fact]
    public async Task GetMissingPage_ShouldReturnNotFoundResponse()
    {
        var response = await appFixture.Client.GetAsync("api/Test/MissingPage", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Message.ShouldBe(ApiMessages.EndpointNotFound);
    }

    [Fact]
    public async Task GetMethodThatThrowsException_ShouldReturnInternalServerErrorResponse()
    {
        var response = await appFixture.Client.GetAsync("api/Test/Exception", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        result.Message.ShouldBe(Messages.InternalError);
        result.TraceId.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetInternalError_ShouldReturnInternalServerErrorResponse()
    {
        var response = await appFixture.Client.GetAsync("api/Test/InternalServerError", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        result.Message.ShouldBe(Messages.InternalError);
        result.TraceId.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetNotFoundError_ShouldReturnNotFoundResponse()
    {
        var response = await appFixture.Client.GetAsync("api/Test/NotFound", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Message.ShouldBe(Messages.NotFound);
    }

    [Fact]
    public async Task GetUnauthorized_ShouldReturnUnauthorizedResponse()
    {
        var response = await appFixture.Client.GetAsync("api/Test/Unauthorized", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.StatusCode.ShouldBe(StatusCodes.Status401Unauthorized);
        result.Message.ShouldBe(Messages.Unauthorized);
    }

    [Fact]
    public async Task GetForbidden_ShouldReturnForbiddenResponse()
    {
        var response = await appFixture.Client.GetAsync("api/Test/Forbidden", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
        result.Message.ShouldBe(Messages.Forbidden);
    }

    [Fact]
    public async Task GetBadRequest_ShouldReturnBadRequestResponse()
    {
        var response = await appFixture.Client.GetAsync("api/Test/BadRequest", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        result.Message.ShouldBe(Messages.BadRequest);
    }
}

