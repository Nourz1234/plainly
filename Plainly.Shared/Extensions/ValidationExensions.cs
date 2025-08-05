using FluentValidation;

namespace Plainly.Shared.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithValidationError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilder, ValidationError errorCode)
    {
        return ruleBuilder.WithErrorCode(errorCode.ToString())
            .WithMessage(errorCode.GetDescription());
    }
}