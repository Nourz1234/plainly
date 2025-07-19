using System.ComponentModel.DataAnnotations;
using Plainly.Api.Interfaces;

namespace Plainly.Api.Abstractions;

public abstract class Entity : IEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
}