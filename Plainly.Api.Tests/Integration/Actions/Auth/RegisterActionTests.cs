using Bogus;
using Plainly.Shared.Actions.Auth.Register;

namespace Plainly.Api.Tests.Integration.Actions.Auth;

public class RegisterActionTests(AppFixture appFixture) : BaseActionTest<RegisterAction, RegisterRequest, RegisterDTO>(appFixture)
{
    protected override HttpMethod Method => HttpMethod.Post;
    protected override string Endpoint => "api/Auth/Register";

    const string StrongPassword = "Test@1234";
    const string WeakPassword = "123456";

    public static RegisterForm GenerateRegisterForm(string? fullName = null, string? email = null, string? password = null, string? confirmPassword = null)
    {
        return new Faker<RegisterForm>()
            .RuleFor(x => x.FullName, f => fullName ?? f.Person.FullName)
            .RuleFor(x => x.Email, f => email ?? f.Person.Email)
            .RuleFor(x => x.Password, f => password ?? StrongPassword)
            .RuleFor(x => x.ConfirmPassword, f => confirmPassword ?? StrongPassword)
            .Generate();
    }

    [Fact]
    public async Task ValidRequest_ShouldSucceed()
    {
        var form = GenerateRegisterForm();
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        // check response
        var result = await GetResultAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Data.Token.ShouldBeOfType<string>();
        result.Data.Token.ShouldNotBeNullOrWhiteSpace();

        // verify that the user was created
        var user = await _AppFixture.UserManager.FindByEmailAsync(form.Email);
        user.ShouldNotBeNull();
        user.FullName.ShouldBe(form.FullName);

        // verify password
        var passwordCheckResult = await _AppFixture.SignInManager.CheckPasswordSignInAsync(user, form.Password, false);
        passwordCheckResult.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public async Task InvalidEmail_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm(email: "invalid email");
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        // check response
        var result = await GetErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(e => e.Field == nameof(RegisterForm.Email) && e.Code == ErrorCode.InvalidEmail.ToString());
    }

    [Fact]
    public async Task EmptyRequiredFields_ShouldReturnValidationError()
    {
        var payload = JsonContent.Create(new { });
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        // error codes map
        (string fieldName, string errorCode)[] fields =
        [
            (nameof(RegisterForm.FullName), ErrorCode.FullNameRequired.ToString()),
            (nameof(RegisterForm.Email), ErrorCode.EmailRequired.ToString()),
            (nameof(RegisterForm.Password), ErrorCode.PasswordRequired.ToString()),
        ];

        // check response
        var result = await GetErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        foreach (var (fieldName, errorCode) in fields)
        {
            result.Errors.ShouldContain(e => e.Field == fieldName && e.Code == errorCode);
        }
    }

    [Fact]
    public async Task DuplicateEmail_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm();
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var errorDescriber = new Microsoft.AspNetCore.Identity.IdentityErrorDescriber();

        var result = await GetErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(e => e.Code == errorDescriber.DuplicateEmail(form.Email).Code);
    }


    [Fact]
    public async Task WeakPassword_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm(password: WeakPassword, confirmPassword: WeakPassword);
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        string[] expectedErrorCodes = [
            ErrorCode.PasswordTooShort.ToString(),
            ErrorCode.PasswordMissingSpecial.ToString(),
            ErrorCode.PasswordMissingUppercase.ToString(),
            ErrorCode.PasswordMissingLowercase.ToString(),
        ];

        var result = await GetErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        foreach (var errorCode in expectedErrorCodes)
        {
            result.Errors.ShouldContain(e => e.Field == nameof(RegisterForm.Password) && e.Code == errorCode.ToString());
        }
    }

    [Fact]
    public async Task PasswordMismatch_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm(password: StrongPassword, confirmPassword: StrongPassword + "1");
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var result = await GetErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(e => e.Field == nameof(RegisterForm.ConfirmPassword) && e.Code == ErrorCode.PasswordMismatch.ToString());
    }
}