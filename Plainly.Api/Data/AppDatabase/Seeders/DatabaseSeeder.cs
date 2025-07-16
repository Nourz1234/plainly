namespace Plainly.Api.Data.AppDatabase.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAllAsync(IServiceProvider services)
    {
        await RoleSeeder.SeedAsync(services);
        await UserSeeder.SeedAsync(services);
    }
}