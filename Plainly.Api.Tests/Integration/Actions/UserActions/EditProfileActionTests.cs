using Plainly.Api.Tests.Integration.Data;
using Plainly.Shared.Actions.User.EditProfile;

namespace Plainly.Api.Tests.Integration.Actions.UserActions;

public class EditProfileActionTests(AppFixture appFixture) : AuthedActionTest<EditProfileAction, EditProfileRequest, EditProfileDTO>(appFixture)
{
    protected override HttpMethod Method => HttpMethod.Patch;
    protected override string Endpoint => "api/User/Profile";


    [Fact]
    public async Task ValidRequest_ShouldSucceed()
    {
        var form = new EditProfileFrom
        {
            FullName = "New Full Name",
        };
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(Users.EditProfileUser, payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await GetResultAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Data.FullName.ShouldBe(form.FullName);

        var userEntity = await _AppFixture.UserManager.FindByEmailAsync(Users.EditProfileUser.Email);
        userEntity.ShouldNotBeNull();
        userEntity.FullName.ShouldBe(form.FullName);
    }
}