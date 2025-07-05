using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
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
        result.Message.ShouldBe(InternalServerErrorException.DefaultMessage);
    }

    [Fact]
    public async Task GetInternalError_ShouldReturnInternalServerErrorResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/internal-error");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Message.ShouldBe(InternalServerErrorException.DefaultMessage);
    }

    [Fact]
    public async Task GetNotFoundError_ShouldReturnNotFoundResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/not-found-error");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Message.ShouldBe(NotFoundException.DefaultMessage);
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

    [Fact]
    public async Task GetValidationError_ShouldReturnValidationErrorResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/validation-error");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Errors.ShouldBeEmpty();
        result.Message.ShouldBe(ValidationException.DefaultMessage);
    }

    [Fact]
    public async Task GetValidationError_WithErrors_ShouldReturnValidationErrorWithErrorsResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/validation-error-with-errors");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBe(false);
        result.Errors.ShouldNotBeEmpty();
        result.Message.ShouldBe(ValidationException.DefaultMessage);
    }

}

