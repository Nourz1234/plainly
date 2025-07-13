using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Exceptions;

namespace Plainly.Api.Infrastructure.ExceptionHandling;


public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(GlobalExceptionHandler);
    }

    private static void GlobalExceptionHandler(IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionFeature?.Error;

            var appException = exception switch
            {
                BaseException baseException => baseException,
                Exception inner => new InternalServerErrorException(null, inner),
                _ => new InternalServerErrorException()
            };

            var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
            var result = appException.ToActionResult(traceId);

            await WriteActionResultAsync(context, result);
        });
    }

    private static async Task WriteActionResultAsync(HttpContext context, IActionResult result)
    {
        await result.ExecuteResultAsync(new ActionContext
        {
            HttpContext = context,
        });
    }
}