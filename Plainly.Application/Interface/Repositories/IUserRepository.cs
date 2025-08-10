using System.Security.Claims;
using Plainly.Domain.Interfaces;

namespace Plainly.Application.Interface.Repositories;


public interface IUserRepository
{
    Task<IUser?> GetUserAsync(ClaimsPrincipal claimsPrincipal);
    Task<IUser?> GetAsync(string id);
    Task<IUser?> GetByEmailAsync(string email);
    Task<IUser> CreateAsync(string fullName, string email, string password);
    Task UpdateAsync(IUser user);
    Task<string[]> GetRolesAsync(IUser user);
    Task AddRoleAsync(IUser user, string role);
    Task RemoveRoleAsync(IUser user, string role);
    Task<Claim[]> GetClaimsAsync(IUser user);
    Task AddClaimAsync(IUser user, Claim claim);
    Task RemoveClaimAsync(IUser user, Claim claim);
}