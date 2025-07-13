using Microsoft.AspNetCore.Mvc;

namespace Plainly.Api.Infrastructure.Web;

public static class HttpContextExtensions
{
    public static async Task WriteActionResultAsync(this HttpContext context, IActionResult result)
    {
        await result.ExecuteResultAsync(new ActionContext
        {
            HttpContext = context,
        });
    }
}