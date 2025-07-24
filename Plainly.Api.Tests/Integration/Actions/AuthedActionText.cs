using Plainly.Api.Tests.Integration.Data;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Tests.Integration.Actions;

public abstract class AuthedActionTest<TAction, TRequest>(AppFixture appFixture)
    : BaseActionTest<TAction, TRequest>(appFixture)
    where TAction : IAction<TRequest>
{
    [Fact]
    public async Task UnauthedUser_ShouldReturnUnauthorized()
    {
        var response = await PerformActionAsync(TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task InsufficientScopes_ShouldReturnForbidden()
    {
        var response = await PerformActionAsync(Users.NoScopesUser, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}

public abstract class AuthedActionTest<TAction, TRequest, TResponse>(AppFixture appFixture)
    : BaseActionTest<TAction, TRequest, TResponse>(appFixture)
    where TAction : IAction<TRequest, TResponse>
{
    [Fact]
    public async Task UnauthedUser_ShouldReturnUnauthorized()
    {
        var response = await PerformActionAsync(TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task InsufficientScopes_ShouldReturnForbidden()
    {
        var response = await PerformActionAsync(Users.NoScopesUser, TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}