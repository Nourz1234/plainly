namespace Plainly.Api.Interfaces;

public interface IBaseModel
{
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
    bool IsDeleted { get; set; }
}
