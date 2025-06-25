using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Plainly.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
public class AppFixture()
{
    private static readonly WebApplicationFactory<Program> Factory = new();

    public readonly HttpClient Client = Factory.CreateClient();

    [Fact]
    public async Task TestMissingPage()
    {
        var response = await Client.GetAsync("/does-not-exist");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TestException()
    {
        var response = await Client.GetAsync("/exception");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task TestInternalError()
    {
        var response = await Client.GetAsync("/internal-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task TestNotFoundError()
    {
        var response = await Client.GetAsync("/not-found-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TestUnauthorized()
    {
        var response = await Client.GetAsync("/unauthorized-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TestForbidden()
    {
        var response = await Client.GetAsync("/forbidden-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TestBadRequest()
    {
        var response = await Client.GetAsync("/bad-request-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task TestValidationError()
    {
        var response = await Client.GetAsync("/validation-error");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task GetValidationErrorWithErrors()
    {
        var response = await Client.GetAsync("/validation-error-with-errors");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);
    }

}

[CollectionDefinition("App collection")]
public class AppCollection : ICollectionFixture<AppFixture>
{
}

