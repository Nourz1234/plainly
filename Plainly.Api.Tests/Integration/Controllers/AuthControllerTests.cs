using System.CodeDom;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Plainly.Shared;
using Plainly.Shared.Actions.Auth.Register;
using Plainly.Shared.Responses;
using Shouldly;

namespace Plainly.Api.Tests.Integration.Controllers;

[ExcludeFromCodeCoverage]
[Collection("App collection")]
public class AuthControllerTests(AppFixture _AppFixture)
{

    [Theory]
    [InlineData("Test User", "test1@plainly.com", "Test@1234", "Test@1234")]
    [InlineData("Test User", "test2@plainly.com", "Test@1234", "Test@1234")]
    [InlineData("Test User", "test3@plainly.com", "Test@1234", "Test@1234")]
    public async Task Register_ValidRequest_ShouldSucceed(string fullName, string email, string password, string confirmPassword)
    {
        var form = new RegisterForm
        {
            FullName = fullName,
            Email = email,
            Password = password,
            ConfirmPassword = confirmPassword
        };
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        // check response
        var result = await response.Content.ReadFromJsonAsync<SuccessResponse<RegisterDTO>>();
        result.ShouldNotBeNull();
        result.Data.Token.ShouldBeOfType<string>();
        result.Data.Token.ShouldNotBeNullOrWhiteSpace();

        // verify that the user was created
        var user = await _AppFixture.UserManager.FindByEmailAsync(email);
        user.ShouldNotBeNull();
        user.FullName.ShouldBe(fullName);

        // verify password
        var passwordCheckResult = await _AppFixture.SignInManager.CheckPasswordSignInAsync(user, password, false);
        passwordCheckResult.Succeeded.ShouldBeTrue();
    }

    [Theory]
    [InlineData("", "test4@plainly.com", "Test@1234", "Test@1234")]
    [InlineData("Test User", "", "Test@1234", "Test@1234")]
    [InlineData("Test User", "test4@plainly.com", "", "")]
    public async Task Register_EmptyRequiredFields_ShouldReturnValidationError(string fullName, string email, string password, string confirmPassword)
    {
        var form = new RegisterForm
        {
            FullName = fullName,
            Email = email,
            Password = password,
            ConfirmPassword = confirmPassword
        };
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();

        (string fieldName, string fieldValue)[] fields =
        [
            (nameof(RegisterForm.FullName),  fullName),
            (nameof(RegisterForm.Email),  email),
            (nameof(RegisterForm.Password),  password),
        ];
        foreach (var (fieldName, fieldValue) in fields)
        {
            if (fieldValue == "")
                result.Errors.Single().Key.ShouldBe(fieldName);
        }
        result.Errors.Single().Value.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Register_DuplicateEmail_ShouldReturnUnprocessableEntity()
    {
        var form = new RegisterForm
        {
            FullName = "Test User",
            Email = "test5@plainly.com",
            Password = "Test@1234",
            ConfirmPassword = "Test@1234"
        };
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        // result.Errors.Single().ShouldBe($"Email '{form.Email}' is already taken.");
    }


    [Fact]
    public async Task Register_WeakPassword_ShouldReturnUnprocessableEntity()
    {
        var form = new RegisterForm
        {
            FullName = "Test User",
            Email = "test6@plainly.com",
            Password = "123456",
            ConfirmPassword = "123456"
        };
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
        result.Errors.Keys.Single().ShouldBe(nameof(RegisterForm.Password));
        result.Errors.Values.Single().ShouldAllBe(e => expectedErrors.Contains(e.ErrorCode));
    }

    [Fact]
    public async Task Register_WeakMismatch_ShouldReturnUnprocessableEntity()
    {
        var form = new RegisterForm
        {
            FullName = "Test User",
            Email = "test6@plainly.com",
            Password = "Test@1234",
            ConfirmPassword = "Test@12345"
        };
        var payload = JsonContent.Create(form);
        var response = await _AppFixture.Client.PostAsync("api/Auth/Register", payload);
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Errors.Single().Key.ShouldBe(nameof(RegisterForm.ConfirmPassword));
        result.Errors.Single().Value.Single().ErrorCode.ShouldBe(ErrorCode.PasswordMismatch.ToString());
    }
}