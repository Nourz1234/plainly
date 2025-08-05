using FluentValidation;
using Plainly.Shared.Extensions;

namespace Plainly.Shared.Actions.User.EditProfile;

public class EditProfileFromValidator : AbstractValidator<EditProfileFrom>
{
    public EditProfileFromValidator()
    {
        When(x => x.FullName != null, () =>
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithValidationError(ValidationError.FullNameRequired);
        });
    }
}
