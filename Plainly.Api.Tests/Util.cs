using System.ComponentModel;
using System.Reflection;

namespace Plainly.Api.Tests;

public static class Util
{
    public static string GetDisplayName<T>(string propertyName)
    {
        var property = typeof(T).GetProperty(propertyName) ?? throw new InvalidOperationException("Property not found.");
        var displayNameAttr = property.GetCustomAttribute<DisplayNameAttribute>()
            ?? throw new InvalidOperationException("Property does not have DisplayNameAttribute.");
        return displayNameAttr.DisplayName;
    }
}