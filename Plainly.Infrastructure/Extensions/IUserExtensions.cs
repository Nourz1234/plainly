using Plainly.Domain.Interfaces;
using Plainly.Infrastructure.Persistence.AppDatabase.Entities;

namespace Plainly.Infrastructure.Extensions;


public static class IUserExtensions
{
    public static User AsEntity(this IUser user)
    {
        return user as User ?? throw new ArgumentException("Must be of type User", nameof(user));
    }
}