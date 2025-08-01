using System.Diagnostics;

namespace Plainly.Api.Extensions;

public static class HttpContextExtensions
{
    public static string GetTraceId(this HttpContext context)
    {
        return Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
    }
}