using Plainly.Api.Tests.Integration.Data;
using Plainly.Shared.Actions.User.ViewProfile;

namespace Plainly.Api.Tests.Integration.Actions.UserActions;

public class ViewProfileActionTests(AppFixture appFixture) : AuthedActionTest<ViewProfileAction, ViewProfileRequest, ViewProfileDTO>(appFixture)
{
    protected override HttpMethod Method => HttpMethod.Get;
    protected override string Endpoint => "api/User/Profile";


    [Fact]
    public async Task ValidRequest_ShouldReturnProfileInfo()
    {
        var response = await PerformActionAsync(Users.TestUser, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await GetResultAsync(response, cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Data.FullName.ShouldBe(Users.TestUser.FullName);
        result.Data.Email.ShouldBe(Users.TestUser.Email);
    }
}