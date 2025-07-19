using System.Reflection;
using System.Runtime.Serialization;

namespace Plainly.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetEnumMemberValue(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString()) ?? throw new ArgumentException("Field not found.");

        EnumMemberAttribute[] attributes =
            (EnumMemberAttribute[])field.GetCustomAttributes(typeof(EnumMemberAttribute), false);

        if (attributes.Length == 0)
            throw new InvalidOperationException("No EnumMemberAttribute found.");

        return attributes[0].Value ?? throw new ArgumentException("EnumMemberAttribute.Value is null.");
    }
}