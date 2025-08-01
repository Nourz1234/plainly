using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Validation;

public static class DependencyInjection
{
    public static IMvcBuilder AddAutoValidation(this IMvcBuilder builder)
    {
        // Add filter
        builder.Services.AddScoped<AutoValidationActionFilter>();
        builder.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.AddService<AutoValidationActionFilter>();
        });

        // Add validators
        builder.Services.Scan(scan =>
        {
            scan.FromAssemblyOf<IAction>()
                .AddClasses(c => c.AssignableTo(typeof(AbstractValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        return builder;
    }
}