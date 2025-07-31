using System.Diagnostics.CodeAnalysis;

namespace Plainly.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
[Collection("App collection")]
public class AppTests(AppFixture appFixture)
{
    [Fact]
    public async Task GetMissingPage_ShouldReturnNotFoundResponse()
    {
        var response = await appFixture.Client.GetAsync("/missing-page", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe(Messages.EndpointNotFound);
    }

    [Fact]
    public async Task GetMethodThatThrowsException_ShouldReturnInternalServerErrorResponse()
    {
        var response = await appFixture.Client.GetAsync("/exception", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe(Messages.InternalServerError);
    }

    [Fact]
    public async Task GetInternalError_ShouldReturnInternalServerErrorResponse()
    {
        var response = await appFixture.Client.GetAsync("/internal-error", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe(Messages.InternalServerError);
    }

    [Fact]
    public async Task GetNotFoundError_ShouldReturnNotFoundResponse()
    {
        var response = await appFixture.Client.GetAsync("/not-found-error", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe(Messages.NotFound);
    }

    [Fact]
    public async Task GetUnauthorized_ShouldReturnUnauthorizedResponse()
    {
        var response = await appFixture.Client.GetAsync("/unauthorized-error", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe(Messages.Unauthorized);
    }

    [Fact]
    public async Task GetForbidden_ShouldReturnForbiddenResponse()
    {
        var response = await appFixture.Client.GetAsync("/forbidden-error", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe(Messages.Forbidden);
    }

    [Fact]
    public async Task GetBadRequest_ShouldReturnBadRequestResponse()
    {
        var response = await appFixture.Client.GetAsync("/bad-request-error", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe(Messages.BadRequest);
    }
}

