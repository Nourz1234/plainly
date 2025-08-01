namespace Plainly.Api.Extensions;

public static class EnvironmentExtensions
{
    public static bool IsTesting(this IWebHostEnvironment environment) => environment.IsEnvironment("Testing");
}