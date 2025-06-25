using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Exceptions;

namespace Plainly.Api.Middleware;


public static class GlobalExceptionHandling
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

            var result = new ObjectResult(appException.ToResponse())
            {
                StatusCode = appException.StatusCode
            };

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