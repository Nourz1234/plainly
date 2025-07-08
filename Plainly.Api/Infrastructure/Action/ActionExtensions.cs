namespace Plainly.Api.Infrastructure.Action;

public static class ActionExtensions
{
    public static void AddActionFactory(this IServiceCollection services)
    {
        services.AddSingleton<ActionHandlerFactory>();
    }
}