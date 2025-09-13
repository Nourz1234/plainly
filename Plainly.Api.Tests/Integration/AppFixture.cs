using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Plainly.Api.Tests.Integration.Data;
using Plainly.Application.Interface;
using Plainly.Infrastructure.Persistence.AppDatabase;
using Plainly.Shared.Extensions;

namespace Plainly.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
public class AppFixture : IAsyncLifetime
{
    private static readonly TestAppFactory Factory = new();

    public readonly HttpClient Client = Factory.CreateClient();
    public readonly AppDbContext DbContext = Factory.Services.GetRequiredService<AppDbContext>();
    public readonly UserManager<Plainly.Infrastructure.Persistence.AppDatabase.Entities.User> UserManager = Factory.Services.GetRequiredService<UserManager<Plainly.Infrastructure.Persistence.AppDatabase.Entities.User>>();
    public readonly SignInManager<Plainly.Infrastructure.Persistence.AppDatabase.Entities.User> SignInManager = Factory.Services.GetRequiredService<SignInManager<Plainly.Infrastructure.Persistence.AppDatabase.Entities.User>>();
    public readonly IJwtService JwtService = Factory.Services.GetRequiredService<IJwtService>();

    public async Task<HttpClient> GetClientForUser(User user)
    {
        var userEntity = await UserManager.FindByEmailAsync(user.Email);
        var token = await JwtService.GenerateToken(userEntity!);
        DbContext.ChangeTracker.Clear();

        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async ValueTask InitializeAsync()
    {
        // TODO: potentially run seeders

        foreach (var user in Users.All)
        {
            var userEntity = new Plainly.Infrastructure.Persistence.AppDatabase.Entities.User
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
                    user.Scopes.Select(s => new Claim(JwtClaimNames.Scopes, s.GetEnumMemberValue())).ToArray()
                );
            }

        }
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

}

[CollectionDefinition("App collection")]
public class AppCollection : ICollectionFixture<AppFixture>
{
}

