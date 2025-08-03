using System.ComponentModel;
using System.Reflection;
using FluentValidation;

namespace Plainly.Api.Validation;

public static class FluentValidationConfig
{
    public static void Configure()
    {
        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
        {
            var displayNameAttribute = memberInfo?.GetCustomAttribute<DisplayNameAttribute>()
                ?? throw new InvalidOperationException($"Form field {type?.Name}.{memberInfo?.Name} is missing {nameof(DisplayNameAttribute)} attribute.");

            return displayNameAttribute.DisplayName;
        };
    }
}