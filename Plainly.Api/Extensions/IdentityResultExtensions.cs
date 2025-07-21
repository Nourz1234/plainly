using Microsoft.AspNetCore.Identity;
using Plainly.Api.Exceptions;
using Plainly.Shared.Responses;

namespace Plainly.Api.Extensions;

public static class IdentityResultExtensions
{
    public static void ThrowIfFailed(this IdentityResult result)
    {
        if (!result.Succeeded)
            throw new ValidationException(errors: new()
            {
                [""] = result.Errors.Select(x => new ValidationErrorDetail(x.Description, x.Code)).ToArray()
            });
    }
}