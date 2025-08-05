using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Shared.Responses;

namespace Plainly.Api.Filters;

/// <summary>
/// Sets the status code of the response
/// </summary>
public class ResponseResultFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult { Value: SuccessResponse response } objectResult)
        {
            objectResult.StatusCode = response.StatusCode;
        }

        await next();
    }
}
