using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
namespace Plainly.Api.Tests;

public class AppFixture(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetMissingPage_Returns404()
    {
        var response = await _client.GetAsync("/does-not-exist");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
