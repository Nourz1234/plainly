using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Builders;
using Plainly.Api.Exceptions;
using Plainly.Api.Extensions;
using Plainly.Domain;
using Plainly.Shared;
using Plainly.Shared.Extensions;
using Plainly.Shared.Responses;

namespace Plainly.Api.ExceptionHandling;

public static class DependencyInjection
{
    public static IServiceCollection AddGlobalExceptionHandling(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory;
        });

        return services;
    }

    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler((app) => app.Run(GlobalExceptionHandler));
    }

    private static async Task GlobalExceptionHandler(HttpContext context)
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionFeature?.Error;

        ErrorResponse response = exception switch
        {
            ApiException apiException => ErrorResponseBuilder.FromApiException(apiException).Build(context),
            DomainError domainError => ErrorResponseBuilder.FromDomainError(domainError).Build(context),
            _ => ErrorResponseBuilder.FromErrorCode(ErrorCode.InternalError).Build(context),
        };

        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }

    private static IActionResult InvalidModelStateResponseFactory(ActionContext context)
    {
        // TODO: Add logging
        var extraFields = context.ModelState
            .Where(x => x.Key.StartsWith("$."))
            .Select(x => x.Key.Replace("$.", ""))
            .ToArray();
        if (extraFields.Length > 0)
        {
            var errors = extraFields.Select(
                field => new ErrorDetail(ValidationError.UnknownField.ToString(), ValidationError.UnknownField.GetDescription(), field)
            ).ToArray();
            return ErrorResponseBuilder.FromErrorCode(ErrorCode.ValidationError)
                .WithErrors(errors)
                .Build(context.HttpContext)
                .ToActionResult();
        }

        return ErrorResponseBuilder.FromErrorCode(ErrorCode.BadRequest).Build(context.HttpContext).ToActionResult();
    }
}