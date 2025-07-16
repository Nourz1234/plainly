using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Plainly.Api.Interfaces;

namespace Plainly.Api.Models;

public class User : IdentityUser, IBaseModel
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    [Required]
    [MinLength(3)]
    [StringLength(64)]
    public required string FullName { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
}