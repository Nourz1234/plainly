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
        var result = await GetValidationErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ContainsKey(nameof(RegisterForm.Email)).ShouldBeTrue();
        result.Errors[nameof(RegisterForm.Email)].ShouldContain(x => x.ErrorCode == ErrorCode.InvalidEmail.ToString());
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
        var result = await GetValidationErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        foreach (var (fieldName, errorCode) in fields)
        {
            result.Errors.ContainsKey(fieldName).ShouldBeTrue();
            var errors = result.Errors[fieldName];
            errors.ShouldNotBeEmpty();
            errors.ShouldContain(x => x.ErrorCode == errorCode);
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

        var result = await GetValidationErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ContainsKey("").ShouldBeTrue();
        result.Errors[""].ShouldContain(x => x.ErrorCode == errorDescriber.DuplicateEmail(form.Email).Code);
    }


    [Fact]
    public async Task WeakPassword_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm(password: WeakPassword, confirmPassword: WeakPassword);
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        string[] expectedErrors = [
            ErrorCode.PasswordTooShort.ToString(),
            ErrorCode.PasswordMissingSpecial.ToString(),
            ErrorCode.PasswordMissingUppercase.ToString(),
            ErrorCode.PasswordMissingLowercase.ToString(),
        ];

        var result = await GetValidationErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ContainsKey(nameof(RegisterForm.Password)).ShouldBeTrue();
        result.Errors[nameof(RegisterForm.Password)].ShouldAllBe(x => expectedErrors.Contains(x.ErrorCode));
    }

    [Fact]
    public async Task PasswordMismatch_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm(password: StrongPassword, confirmPassword: StrongPassword + "1");
        var payload = JsonContent.Create(form);
        var response = await PerformActionAsync(payload, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var result = await GetValidationErrorAsync(response, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ContainsKey(nameof(RegisterForm.ConfirmPassword)).ShouldBeTrue();
        result.Errors[nameof(RegisterForm.ConfirmPassword)].ShouldContain(x => x.ErrorCode == ErrorCode.PasswordMismatch.ToString());
    }


}