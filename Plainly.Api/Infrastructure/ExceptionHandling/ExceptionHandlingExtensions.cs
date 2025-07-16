using Microsoft.AspNetCore.Diagnostics;
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
        app.Run(async context =>
        {
            var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionFeature?.Error;
            var traceId = context.GetTraceId();

            ErrorResponse response = exception switch
            {
                BaseException baseException => baseException.ToResponse(traceId),
                _ => new ErrorResponse { Message = Messages.InternalServerError, TraceId = traceId },
            };

            await context.Response.WriteAsJsonAsync(response);
        });
    }
}