using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using Bogus;
using Plainly.Api.Tests.Integration.Data;
using Plainly.Shared;
using Plainly.Shared.Actions.Auth.Login;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Responses;
using Shouldly;

namespace Plainly.Api.Tests.Integration.Controllers;

[ExcludeFromCodeCoverage]
[Collection("App collection")]
public class AuthControllerTests(AppFixture appFixture)
{
    private readonly AppFixture _AppFixture = appFixture;
    const string StrongPassword = "Test@1234";
    const string WeakPassword = "123456";

    public static RegisterForm GenerateRegisterForm_Valid()
    {
        return new Faker<RegisterForm>()
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.Password, f => StrongPassword)
            .RuleFor(x => x.ConfirmPassword, f => StrongPassword)
            .Generate();
    }

    public static RegisterForm GenerateRegisterForm_InvalidEmail()
    {
        return new Faker<RegisterForm>()
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => "invalid email")
            .RuleFor(x => x.Password, f => StrongPassword)
            .RuleFor(x => x.ConfirmPassword, f => StrongPassword)
            .Generate();
    }

    public static RegisterForm GenerateRegisterForm_WeakPassword()
    {
        return new Faker<RegisterForm>()
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.Password, f => WeakPassword)
            .RuleFor(x => x.ConfirmPassword, f => WeakPassword)
            .Generate();
    }

    public static RegisterForm GenerateRegisterForm_PasswordMismatch()
    {
        return new Faker<RegisterForm>()
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.Password, f => StrongPassword)
            .RuleFor(x => x.ConfirmPassword, f => StrongPassword + "1")
            .Generate();
    }



    [Fact]
    public async Task Register_ValidRequest_ShouldSucceed()
    {
        var form = GenerateRegisterForm_Valid();
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        // check response
        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<RegisterDTO>>();
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
    public async Task Register_InvalidEmail_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm_InvalidEmail();
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        // check response
        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ContainsKey(nameof(RegisterForm.Email)).ShouldBeTrue();
        result.Errors[nameof(RegisterForm.Email)].ShouldContain(x => x.ErrorCode == ErrorCode.InvalidEmail.ToString());
    }

    [Fact]
    public async Task Register_EmptyRequiredFields_ShouldReturnValidationError()
    {
        var payload = JsonContent.Create(new { });
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        // error codes map
        (string fieldName, string errorCode)[] fields =
        [
            (nameof(RegisterForm.FullName), ErrorCode.FullNameRequired.ToString()),
            (nameof(RegisterForm.Email), ErrorCode.EmailRequired.ToString()),
            (nameof(RegisterForm.Password), ErrorCode.PasswordRequired.ToString()),
        ];

        // check response
        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
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
    public async Task Register_DuplicateEmail_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm_Valid();
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var errorDescriber = new Microsoft.AspNetCore.Identity.IdentityErrorDescriber();

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ContainsKey("").ShouldBeTrue();
        result.Errors[""].ShouldContain(x => x.ErrorCode == errorDescriber.DuplicateEmail(form.Email).Code);
    }


    [Fact]
    public async Task Register_WeakPassword_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm_WeakPassword();
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        string[] expectedErrors = [
            ErrorCode.PasswordTooShort.ToString(),
            ErrorCode.PasswordMissingSpecial.ToString(),
            ErrorCode.PasswordMissingUppercase.ToString(),
            ErrorCode.PasswordMissingLowercase.ToString(),
        ];

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ContainsKey(nameof(RegisterForm.Password)).ShouldBeTrue();
        result.Errors[nameof(RegisterForm.Password)].ShouldAllBe(x => expectedErrors.Contains(x.ErrorCode));
    }

    [Fact]
    public async Task Register_PasswordMismatch_ShouldReturnValidationError()
    {
        var form = GenerateRegisterForm_PasswordMismatch();
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ContainsKey(nameof(RegisterForm.ConfirmPassword)).ShouldBeTrue();
        result.Errors[nameof(RegisterForm.ConfirmPassword)].ShouldContain(x => x.ErrorCode == ErrorCode.PasswordMismatch.ToString());
    }

    [Fact]
    public async Task Login_AdminUser_ShouldSucceed()
    {
        var form = new LoginForm
        {
            Email = Users.AdminUser.Email,
            Password = Users.AdminUser.Password
        };
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Login", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        // check response
        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<LoginDTO>>();
        result.ShouldNotBeNull();
        result.Data.Token.ShouldBeOfType<string>();
        result.Data.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_TestUser_ShouldSucceed()
    {
        var form = new LoginForm
        {
            Email = Users.TestUser.Email,
            Password = Users.TestUser.Password
        };
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Login", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        // check response
        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<LoginDTO>>();
        result.ShouldNotBeNull();
        result.Data.Token.ShouldBeOfType<string>();
        result.Data.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_InActiveUser_ShouldReturnUnauthorized()
    {
        var form = new LoginForm
        {
            Email = Users.InActiveUser.Email,
            Password = Users.InActiveUser.Password
        };
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Login", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        // check response
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe(Messages.UserIsNotActive);
    }

    [Fact]
    public async Task Login_InvalidEmail_ShouldReturnValidationError()
    {
        var form = new LoginForm
        {
            Email = "invalid-email",
            Password = Users.TestUser.Password
        };
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Login", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        // check response
        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ContainsKey(nameof(LoginForm.Email)).ShouldBeTrue();
        result.Errors[nameof(LoginForm.Email)].ShouldContain(x => x.ErrorCode == ErrorCode.InvalidEmail.ToString());
    }


    [Fact]
    public async Task Login_InvalidCredentials_ShouldReturnUnauthorized()
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
            var response = await _AppFixture.Client.PostAsync("api/Auth/Login", payload);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            // check response
            var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe(Messages.InvalidLoginCredentials);
        }
    }
}