using Plainly.Domain.Interfaces;

namespace Plainly.Application.Interface;

public interface ICurrentUserProvider
{
    Task<IUser?> GetCurrentUserAsync();
    Task<IUser?> GetCurrentAdminUserAsync();
}