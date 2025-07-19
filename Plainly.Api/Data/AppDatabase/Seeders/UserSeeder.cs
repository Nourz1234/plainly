using Microsoft.AspNetCore.Identity;
using Plainly.Api.Entities;
using Plainly.Api.Infrastructure.Authorization;

namespace Plainly.Api.Data.AppDatabase.Seeders;

public static class UserSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        userManager.PasswordValidators.Clear(); // no password validation

        (string fullName, string email, string role)[] users = [
            (fullName: "Admin", email: "admin@plainly.com", role: Roles.Admin.Name)
        ];
        const string password = "123456"; // TODO: Move to config

        foreach (var (fullName, email, role) in users)
        {
            var userModel = await userManager.FindByEmailAsync(email);
            if (userModel is null)
            {
                userModel = new User
                {
                    FullName = fullName,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(userModel, password);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create user: {fullName} - error: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(userModel, role))
            {
                await userManager.AddToRoleAsync(userModel, role);
            }
        }
    }
}