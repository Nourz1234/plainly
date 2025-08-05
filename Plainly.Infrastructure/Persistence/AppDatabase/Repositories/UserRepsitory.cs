using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Plainly.Domain.Interfaces;
using Plainly.Domain.Interfaces.Repositories;
using Plainly.Infrastructure.Extensions;
using Plainly.Infrastructure.Persistence.AppDatabase.Entities;

namespace Plainly.Infrastructure.Persistence.AppDatabase.Repositories;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<IUser> CreateAsync(string fullName, string email, string password)
    {
        var userEntity = new User
        {
            FullName = fullName,
            Email = email,
        };
        var result = await userManager.CreateAsync(userEntity, password);
        result.ThrowIfFailed();
        return userEntity;
    }

    public async Task<IUser?> GetAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public async Task<IUser?> GetByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<IUser?> GetUserAsync(ClaimsPrincipal claimsPrincipal)
    {
        return await userManager.GetUserAsync(claimsPrincipal);
    }
    public async Task<string[]> GetRolesAsync(IUser user)
    {
        return (await userManager.GetRolesAsync(user.AsEntity())).ToArray();
    }

    public Task AddRoleAsync(IUser user, string role)
    {
        return userManager.AddToRoleAsync(user.AsEntity(), role);
    }

    public async Task RemoveRoleAsync(IUser user, string role)
    {
        await userManager.RemoveFromRoleAsync(user.AsEntity(), role);
    }

    public Task AddClaimAsync(IUser user, Claim claim)
    {
        return userManager.AddClaimAsync(user.AsEntity(), claim);
    }

    public async Task<Claim[]> GetClaimsAsync(IUser user)
    {
        return (await userManager.GetClaimsAsync(user.AsEntity())).ToArray();
    }

    public async Task RemoveClaimAsync(IUser user, Claim claim)
    {
        var result = await userManager.RemoveClaimAsync(user.AsEntity(), claim);
        result.ThrowIfFailed();
    }

    public async Task UpdateAsync(IUser user)
    {
        var result = await userManager.UpdateAsync(user.AsEntity());
        result.ThrowIfFailed();
    }
}