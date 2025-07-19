using Microsoft.AspNetCore.Identity;
using Plainly.Api.Interfaces;

namespace Plainly.Api.Entities;

public class User : IdentityUser, IEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public required string FullName { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }

    public override string? UserName { get; set; } = Guid.NewGuid().ToString();
}