using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using FluentAssertions;
using Plainly.Api.Exceptions;
using Plainly.Shared;
using Plainly.Shared.Responses;

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
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(Messages.EndpointNotFound);
    }

    [Fact]
    public async Task GetMethodThatThrowsException_ShouldReturnInternalServerErrorResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/exception");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(InternalServerErrorException.DefaultMessage);
    }

    [Fact]
    public async Task GetInternalError_ShouldReturnInternalServerErrorResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/internal-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(InternalServerErrorException.DefaultMessage);
    }

    [Fact]
    public async Task GetNotFoundError_ShouldReturnNotFoundResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/not-found-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(NotFoundException.DefaultMessage);
    }

    [Fact]
    public async Task GetUnauthorized_ShouldReturnUnauthorizedResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/unauthorized-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(UnauthorizedException.DefaultMessage);
    }

    [Fact]
    public async Task GetForbidden_ShouldReturnForbiddenResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/forbidden-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(ForbiddenException.DefaultMessage);
    }

    [Fact]
    public async Task GetBadRequest_ShouldReturnBadRequestResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/bad-request-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(BadRequestException.DefaultMessage);
    }

    [Fact]
    public async Task GetValidationError_ShouldReturnValidationErrorResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/validation-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Errors.Should().BeEmpty();
        result.Message.Should().Be(ValidationException.DefaultMessage);
    }

    [Fact]
    public async Task GetValidationError_WithErrors_ShouldReturnValidationErrorWithErrorsResponse()
    {
        var response = await _AppFixture.Client.GetAsync("/validation-error-with-errors");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Errors.Should().NotBeEmpty();
        result.Message.Should().Be(ValidationException.DefaultMessage);
    }

}

