using Plainly.Api.Tests.Integration.Data;
using Plainly.Domain;
using Plainly.Shared.Actions.Auth.Login;

namespace Plainly.Api.Tests.Integration.Actions.Auth;

public class LoginActionTests(AppFixture appFixture) : BaseActionTest<LoginAction, LoginRequest, LoginDTO>(appFixture)
{
    protected override HttpMethod Method => HttpMethod.Post;
    protected override string Endpoint => "api/Auth/Login";

    [Fact]
    public async Task AdminUser_ShouldSucceed()
    {
        var form = new LoginForm
        {
            Email = Users.AdminUser.Email,
            Password = Users.AdminUser.Password
        };
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        // check response
        var result = await GetResultAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Data.Token.ShouldBeOfType<string>();
        result.Data.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task TestUser_ShouldSucceed()
    {
        var form = new LoginForm
        {
            Email = Users.TestUser.Email,
            Password = Users.TestUser.Password
        };
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        // check response
        var result = await GetResultAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Data.Token.ShouldBeOfType<string>();
        result.Data.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task InActiveUser_ShouldReturnUnauthorized()
    {
        var form = new LoginForm
        {
            Email = Users.InActiveUser.Email,
            Password = Users.InActiveUser.Password
        };
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);

        // check response
        var result = await GetErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe(DomainMessages.UserIsNotActive);
    }

    [Fact]
    public async Task InvalidEmail_ShouldReturnValidationError()
    {
        var form = new LoginForm
        {
            Email = "invalid-email",
            Password = Users.TestUser.Password
        };
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        // check response
        var result = await GetErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(e => e.Field == nameof(LoginForm.Email) && e.Code == ValidationError.InvalidEmail.ToString());
    }

    [Fact]
    public async Task InvalidCredentials_ShouldReturnUnauthorized()
    {
        (string email, string password)[] creds = [
            (Users.TestUser.Email, "invalid-password"),
            ("test@example.com", Users.TestUser.Password),
        ];

        foreach (var (email, password) in creds)
        {
            var form = new LoginForm
            {
                Email = email,
                Password = password
            };
            var payload = JsonContent.Create(form);
            var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            // check response
            var result = await GetErrorAsync(response, TestContext.Current.CancellationToken);
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe(DomainMessages.InvalidLoginCredentials);
        }
    }
}