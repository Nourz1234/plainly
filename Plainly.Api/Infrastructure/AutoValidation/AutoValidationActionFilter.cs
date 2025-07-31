using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Plainly.Api.Infrastructure.Web;
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
                var errors = result.Errors.Select(e => new ErrorDetail(e.ErrorCode, e.ErrorMessage, e.PropertyName)).ToArray();

                context.Result = ErrorResponse.ValidationError()
                    .WithErrors(errors)
                    .WithTraceId(context.HttpContext.GetTraceId())
                    .Build()
                    .ToActionResult();
                return;
            }
        }

        await next();
    }
}