using Microsoft.AspNetCore.Identity;
using Plainly.Domain.Interfaces;

namespace Plainly.Infrastructure.Persistence.AppDatabase.Entities;

public class User : IdentityUser, IUser
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public required string FullName { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public override string? UserName { get; set; } = Guid.NewGuid().ToString();
}