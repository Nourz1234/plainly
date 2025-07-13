using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Shared;
using Plainly.Shared.Responses;

namespace Plainly.Api.Infrastructure.AutoValidation.Filters;

public class AutoValidationActionFilter(IServiceProvider _ServiceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var model in context.ActionArguments.Values)
        {
            if (model is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
            if (_ServiceProvider.GetService(validatorType) is not IValidator validator)
                continue;

            var validationContext = new ValidationContext<object>(model);

            var result = validator.Validate(validationContext);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => new ValidationErrorDetail(e.ErrorMessage, e.ErrorCode)).ToArray()
                    );

                var response = new ValidationErrorResponse
                {
                    Message = Messages.ValidationError,
                    Errors = errors
                };

                context.Result = response.Convert();
                return;
            }
        }

        await next();
    }
}