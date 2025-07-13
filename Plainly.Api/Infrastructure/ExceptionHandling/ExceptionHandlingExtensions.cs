using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Exceptions;
using Plainly.Api.Infrastructure.Web;
using Plainly.Shared;
using Plainly.Shared.Responses;

namespace Plainly.Api.Infrastructure.ExceptionHandling;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(GlobalExceptionHandler);
    }

    private static void GlobalExceptionHandler(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionFeature?.Error;

            var appException = exception switch
            {
                BaseException baseException => baseException,
                Exception inner => new InternalServerErrorException(null, inner),
                _ => new InternalServerErrorException()
            };

            var result = appException.ToActionResult();

            await context.WriteActionResultAsync(result);
        });

    }
}