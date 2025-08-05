using Plainly.Api.Attributes;
using Plainly.Shared.Extensions;

namespace Plainly.Api.Extensions;

public static class EnumExtensions
{
    public static int GetStatusCode(this Enum value)
    {
        return value.GetAttribute<StatusCodeAttribute>().StatusCode;
    }
}