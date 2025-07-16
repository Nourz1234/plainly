using System.Diagnostics;

namespace Plainly.Api.Infrastructure.Web;

public static class HttpContextExtensions
{
    public static string GetTraceId(this HttpContext context)
    {
        return Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
    }
}