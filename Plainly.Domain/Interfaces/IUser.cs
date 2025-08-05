namespace Plainly.Domain.Interfaces;

/// <summary>
/// We declare the user entity in the domain as an interface since we are using asp.NET Identity
/// and we can't have an infrastructure dependency
/// </summary>
public interface IUser : IEntity
{
    string Id { get; set; }
    string FullName { get; set; }
    string? Email { get; set; }
    bool IsActive { get; set; }
    DateTime? LastLoginAt { get; set; }
}