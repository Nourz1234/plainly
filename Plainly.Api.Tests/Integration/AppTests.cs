using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text;
using Plainly.Api.Exceptions;
using Plainly.Shared;
using Plainly.Shared.Responses;
using Shouldly;

namespace Plainly.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
[Collection("App collection")]
public class AppTests(AppFixture appFixture)
{
    private readonly AppFixture _AppFixture = appFixture;

    [Fact]
    public async Task GetMissingPage_ShouldReturnNotFoundResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/missing-page");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Message.ShouldBe(Messages.EndpointNotFound);
    }

    [Fact]
    public async Task GetMethodThatThrowsException_ShouldReturnInternalServerErrorResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/exception");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Message.ShouldBe(Messages.InternalServerError);
    }

    [Fact]
    public async Task GetInternalError_ShouldReturnInternalServerErrorResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/internal-error");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Message.ShouldBe(Messages.InternalServerError);
    }

    [Fact]
    public async Task GetNotFoundError_ShouldReturnNotFoundResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/not-found-error");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Message.ShouldBe(Messages.NotFound);
    }

    [Fact]
    public async Task GetUnauthorized_ShouldReturnUnauthorizedResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/unauthorized-error");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Message.ShouldBe(UnauthorizedException.DefaultMessage);
    }

    [Fact]
    public async Task GetForbidden_ShouldReturnForbiddenResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/forbidden-error");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Forbidden);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Message.ShouldBe(ForbiddenException.DefaultMessage);
    }

    [Fact]
    public async Task GetBadRequest_ShouldReturnBadRequestResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/bad-request-error");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Message.ShouldBe(BadRequestException.DefaultMessage);
    }
}

