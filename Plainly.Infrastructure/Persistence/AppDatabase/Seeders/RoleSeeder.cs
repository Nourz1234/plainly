using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Plainly.Shared;
using Plainly.Shared.Extensions;

namespace Plainly.Infrastructure.Persistence.AppDatabase.Seeders;

public static class RoleSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(new IdentityRole(role.Name));
            }

            if (role.Scopes is not null)
            {
                foreach (var scope in role.Scopes)
                    await roleManager.AddClaimAsync(new IdentityRole(role.Name), new Claim(JwtClaimNames.Scopes, scope.GetEnumMemberValue()));
            }
        }
    }
}