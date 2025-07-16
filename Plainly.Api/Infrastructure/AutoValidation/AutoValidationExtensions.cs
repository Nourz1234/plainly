using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Infrastructure.AutoValidation.Filters;

namespace Plainly.Api.Infrastructure.AutoValidation;

public static class AutoValidationExtensions
{
    public static IMvcBuilder AddAutoValidation(this IMvcBuilder builder)
    {
        builder.Services.AddScoped<AutoValidationActionFilter>();
        builder.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.AddService<AutoValidationActionFilter>();
        });

        return builder;
    }

    public static IMvcBuilder AddValidatorsFromAssemblyOf<T>(this IMvcBuilder builder)
    {
        builder.Services.Scan(scan =>
        {
            scan.FromAssemblyOf<T>()
                .AddClasses(c => c.AssignableTo(typeof(AbstractValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        return builder;
    }
}