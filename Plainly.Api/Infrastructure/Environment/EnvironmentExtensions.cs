namespace Plainly.Api.Infrastructure.Environment;


public static class EnvironmentExtensions
{
    public static bool IsTesting(this IWebHostEnvironment environment) => environment.IsEnvironment("Testing");
}