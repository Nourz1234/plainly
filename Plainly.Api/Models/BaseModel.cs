using Plainly.Api.Interfaces;

namespace Plainly.Api.Models;


public abstract class BaseModel : IBaseModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
}