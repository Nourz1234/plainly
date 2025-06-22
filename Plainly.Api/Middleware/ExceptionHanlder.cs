using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Exceptions;

namespace Plainly.Api.Middleware;


public static class ExceptionHandler
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
                Exception inner => new InternalErrorException(null, inner),
                _ => new InternalErrorException()
            };

            var result = new ObjectResult(appException.GetResult())
            {
                StatusCode = StatusCodes.Status500InternalServerError
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