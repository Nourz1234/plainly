using Microsoft.AspNetCore.Identity;
using Plainly.Domain;
using Plainly.Domain.Exceptions;
using Plainly.Shared.Responses;

namespace Plainly.Infrastructure.Extensions;

public static class IdentityResultExtensions
{
    public static void ThrowIfFailed(this IdentityResult result)
    {
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => new ErrorDetail(error.Code, error.Description)).ToArray();
            throw new DomainException(DomainErrorCode.ValidationError, errors);
        }
    }
}