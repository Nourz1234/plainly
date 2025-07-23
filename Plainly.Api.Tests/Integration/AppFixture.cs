using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Plainly.Api.Data.AppDatabase;
using Plainly.Api.Infrastructure.Jwt;
using Plainly.Api.Tests.Integration.Data;
using Plainly.Shared.Extensions;

namespace Plainly.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
public class AppFixture : IAsyncLifetime
{
    private static readonly TestAppFactory Factory = new();

    public readonly HttpClient Client = Factory.CreateClient();
    public readonly AppDbContext DbContext = Factory.Services.GetRequiredService<AppDbContext>();
    public readonly UserManager<Entities.User> UserManager = Factory.Services.GetRequiredService<UserManager<Entities.User>>();
    public readonly SignInManager<Entities.User> SignInManager = Factory.Services.GetRequiredService<SignInManager<Entities.User>>();
    public readonly JwtService JwtService = Factory.Services.GetRequiredService<JwtService>();

    public async Task<HttpClient> GetClientForUser(User user)
    {
        var client = Factory.CreateClient();
        var userEntity = await UserManager.FindByEmailAsync(user.Email);
        var token = await JwtService.GenerateToken(userEntity!);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async Task InitializeAsync()
    {
        // TODO: potentially run seeders

        var testUsers = new User[] { Users.TestUser, Users.NoScopesUser, Users.InActiveUser };
        foreach (var user in testUsers)
        {
            var userEntity = new Entities.User
            {
                FullName = user.FullName,
                Email = user.Email,
                EmailConfirmed = true,
                IsActive = user.IsActive
            };
            await UserManager.CreateAsync(userEntity, user.Password);
            if (user.Scopes is { Length: > 0 })
            {
                await UserManager.AddClaimsAsync(
                    userEntity,
                    user.Scopes.Select(s => new Claim("scopes", s.GetEnumMemberValue())).ToArray()
                );
            }

        }
    }

    public Task DisposeAsync() => Task.CompletedTask;

}

[CollectionDefinition("App collection")]
public class AppCollection : ICollectionFixture<AppFixture>
{
}

