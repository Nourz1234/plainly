using System.ComponentModel.DataAnnotations;
using Plainly.Domain.Interfaces;

namespace Plainly.Domain.Abstractions;

public abstract class Entity : IEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}