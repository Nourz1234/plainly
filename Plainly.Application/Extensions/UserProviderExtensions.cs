using Plainly.Application.Interface;
using Plainly.Domain;
using Plainly.Domain.Exceptions;
using Plainly.Domain.Interfaces;

namespace Plainly.Application.Extensions;

public static class UserProviderExtensions
{
    public static async Task<IUser> GetCurrentUserOrFailAsync(this ICurrentUserProvider userProvider)
        => await userProvider.GetCurrentUserAsync() ?? throw new DomainException(DomainErrorCode.Unauthorized);
}