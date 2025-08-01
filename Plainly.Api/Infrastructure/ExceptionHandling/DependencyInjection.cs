using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Extensions;
using Plainly.Shared;
using Plainly.Shared.Interfaces;
using Plainly.Shared.Responses;

namespace Plainly.Api.Infrastructure.ExceptionHandling;

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
        var traceId = context.GetTraceId();

        ErrorResponse response = exception switch
        {
            IHttpException baseException => ErrorResponse.FromException(baseException).Build(traceId),
            _ => ErrorResponse.InternalServerError().Build(traceId),
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
                field => new ErrorDetail(ErrorCode.UnknownField.ToString(), string.Format(Messages.UnknownField, field))
            ).ToArray();
            return ErrorResponse.ValidationError()
                .WithErrors(errors)
                .Build(context.HttpContext.GetTraceId())
                .ToActionResult();
        }

        return ErrorResponse.BadRequest().Build(context.HttpContext.GetTraceId()).ToActionResult();
    }
}