using System.ComponentModel;

namespace Plainly.Shared.Actions.User.EditProfile;

public record EditProfileFrom
{
    [DisplayName("Full Name")]
    public string? FullName { get; init; }
}
