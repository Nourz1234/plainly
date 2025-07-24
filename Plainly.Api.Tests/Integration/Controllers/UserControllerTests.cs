using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using Plainly.Api.Tests.Integration.Data;
using Plainly.Shared.Actions.User.ViewProfile;
using Plainly.Shared.Responses;
using Shouldly;

namespace Plainly.Api.Tests.Integration.Controllers;

[ExcludeFromCodeCoverage]
[Collection("App collection")]
public class UserControllerTests(AppFixture appFixture)
{
    [Fact]
    public async Task ViewProfile_AuthedUser_ShouldReturnProfileInfo()
    {
        var client = await appFixture.GetClientForUser(Users.TestUser);
        var response = await client.GetAsync("api/User/Profile", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<ViewProfileDTO>>(cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Data.FullName.ShouldBe(Users.TestUser.FullName);
        result.Data.Email.ShouldBe(Users.TestUser.Email);
    }

    [Fact]
    public async Task ViewProfile_InsufficientScopes_ShouldReturnForbidden()
    {
        var client = await appFixture.GetClientForUser(Users.NoScopesUser);
        var response = await client.GetAsync("api/User/Profile", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ViewProfile_UnauthedUser_ShouldReturnUnauthorized()
    {
        var response = await appFixture.Client.GetAsync("api/User/Profile", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
