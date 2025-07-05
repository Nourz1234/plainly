using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using Plainly.Shared.DTOs;
using Plainly.Shared.Responses;
using Shouldly;

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
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<HealthDTO>>();
        result.ShouldNotBeNull();
        result.Data.Status.ShouldBe("Healthy");
    }
}