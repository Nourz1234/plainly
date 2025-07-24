using Plainly.Shared.Actions.Heath.GetHealth;

namespace Plainly.Api.Tests.Integration.Actions.Health;

public class GetHealthActionTests(AppFixture appFixture) : BaseActionTest<GetHealthAction, GetHealthRequest, GetHealthDTO>(appFixture)
{
    protected override HttpMethod Method => HttpMethod.Get;

    protected override string Endpoint => "api/Health";

    [Fact]
    public async Task ShouldReturnHealthyResponse()
    {
        var response = await PerformActionAsync(TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await GetResultAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Data.Status.ShouldBe("Healthy");
    }
}