using System.Diagnostics.CodeAnalysis;

namespace Plainly.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
public class AppFixture()
{
    private static readonly TestAppFactory Factory = new();

    public readonly HttpClient Client = Factory.CreateClient();
}

[CollectionDefinition("App collection")]
public class AppCollection : ICollectionFixture<AppFixture>
{
}

