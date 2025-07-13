using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Plainly.Api.Database;
using Plainly.Api.Models;

namespace Plainly.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
public class AppFixture()
{
    private static readonly TestAppFactory Factory = new();

    public readonly HttpClient Client = Factory.CreateClient();
    public readonly AppDbContext DbContext = Factory.Services.GetRequiredService<AppDbContext>();
    public readonly UserManager<User> UserManager = Factory.Services.GetRequiredService<UserManager<User>>();
    public readonly SignInManager<User> SignInManager = Factory.Services.GetRequiredService<SignInManager<User>>();
}

[CollectionDefinition("App collection")]
public class AppCollection : ICollectionFixture<AppFixture>
{
}

