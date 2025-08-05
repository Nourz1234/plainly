using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Plainly.Application.Interface;
using Plainly.Infrastructure.Persistence.AppDatabase;
using Plainly.Infrastructure.Persistence.AppDatabase.Entities;

namespace Plainly.Infrastructure.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        // Add Identity
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            // Should handle password validation with FluentValidation
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 1;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });


        services.AddHttpContextAccessor();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        return services;
    }
}