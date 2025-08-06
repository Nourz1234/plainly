using Plainly.Domain.Attributes;
using Plainly.Shared.Extensions;

namespace Plainly.Domain.Extensions;

public static class EnumExtensions
{
    public static DomainErrorType GetErrorType(this Enum value)
    {
        return value.GetAttribute<ErrorTypeAttribute>().ErrorType;
    }
}