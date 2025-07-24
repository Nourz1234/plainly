using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using Plainly.Shared.Actions.Heath.GetHealth;
using Plainly.Shared.Responses;
using Shouldly;

namespace Plainly.Api.Tests.Integration.Controllers;

[ExcludeFromCodeCoverage]
[Collection("App collection")]
public class HealthControllerTests(AppFixture _AppFixture)
{
    [Fact]
    public async Task Get_ShouldReturnHealthyResponse()
    {
        var response = await _AppFixture.Client.GetAsync("api/Health", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<GetHealthDTO>>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Data.Status.ShouldBe("Healthy");
    }
}