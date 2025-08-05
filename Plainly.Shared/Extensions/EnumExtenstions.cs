using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace Plainly.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetEnumMemberValue(this Enum value)
    {
        return GetAttribute<EnumMemberAttribute>(value).Value ?? throw new ArgumentException("EnumMemberAttribute.Value is null.");
    }

    public static string GetDescription(this Enum value)
    {
        return GetAttribute<DescriptionAttribute>(value).Description;
    }

    public static string GetCategory(this Enum value)
    {
        return GetAttribute<CategoryAttribute>(value).Category;
    }

    public static T GetAttribute<T>(this Enum value) where T : Attribute
    {
        FieldInfo field = value.GetType().GetField(value.ToString()) ?? throw new ArgumentException("Field not found.");
        return field.GetCustomAttribute<T>() ?? throw new InvalidOperationException($"The attribute {nameof(T)} was not found.");
    }
}