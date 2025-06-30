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
    public async Task TestMissingPage()
    {
        var response = await _AppFixture.Client.GetAsync("/does-not-exist");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(Messages.EndpointNotFoundMessage);
    }

    [Fact]
    public async Task TestException()
    {
        var response = await _AppFixture.Client.GetAsync("/exception");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(InternalServerErrorException.DefaultMessage);
    }

    [Fact]
    public async Task TestInternalError()
    {
        var response = await _AppFixture.Client.GetAsync("/internal-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(InternalServerErrorException.DefaultMessage);
    }

    [Fact]
    public async Task TestNotFoundError()
    {
        var response = await _AppFixture.Client.GetAsync("/not-found-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(NotFoundException.DefaultMessage);
    }

    [Fact]
    public async Task TestUnauthorized()
    {
        var response = await _AppFixture.Client.GetAsync("/unauthorized-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(UnauthorizedException.DefaultMessage);
    }

    [Fact]
    public async Task TestForbidden()
    {
        var response = await _AppFixture.Client.GetAsync("/forbidden-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(ForbiddenException.DefaultMessage);
    }

    [Fact]
    public async Task TestBadRequest()
    {
        var response = await _AppFixture.Client.GetAsync("/bad-request-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result.Success.Should().Be(false);
        result.Message.Should().Be(BadRequestException.DefaultMessage);
    }

    [Fact]
    public async Task TestValidationError()
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
    public async Task GetValidationErrorWithErrors()
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

