using Microsoft.AspNetCore.Identity;
using Plainly.Api.Exceptions;
using Plainly.Shared.Responses;

namespace Plainly.Api.Extensions;

public static class IdentityResultExtensions
{
    public static void ThrowIfFailed(this IdentityResult result)
    {
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => new ErrorDetail(error.Code, error.Description)).ToArray();
            throw new ValidationException(errors);
        }
    }
}