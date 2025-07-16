using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Api.Infrastructure.Web;
using Plainly.Shared;
using Plainly.Shared.Responses;

namespace Plainly.Api.Infrastructure.AutoValidation;

public class AutoValidationActionFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var model in context.ActionArguments.Values)
        {
            if (model is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
            if (serviceProvider.GetService(validatorType) is not IValidator validator)
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

                context.Result = new ValidationErrorResponse
                {
                    Message = Messages.ValidationError,
                    Errors = errors,
                    TraceId = context.HttpContext.GetTraceId()
                }.Convert();
                return;
            }
        }

        await next();
    }
}