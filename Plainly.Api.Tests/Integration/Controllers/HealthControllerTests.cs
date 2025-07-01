using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using FluentAssertions;
using Plainly.Shared.DTOs;
using Plainly.Shared.Responses;

namespace Plainly.Api.Tests.Integration.Controllers;

[ExcludeFromCodeCoverage]
[Collection("App collection")]
public class HealthControllerTests(AppFixture appFixture)
{
    private readonly AppFixture _AppFixture = appFixture;

    [Fact]
    public async Task Get_ShouldReturnHealthyResponse()
    {
        var response = await _AppFixture.Client.GetAsync("api/Health");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<HealthDTO>>();
        result.Should().NotBeNull();
        result.Data.Status.Should().Be("Healthy");
    }
}