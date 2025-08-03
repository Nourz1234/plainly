using Microsoft.AspNetCore.Identity;
using Plainly.Api.Extensions;
using Plainly.Api.Infrastructure.Identity;
using Plainly.Shared.Actions.User.EditProfile;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions.User;

public class EditProfileActionHandler(UserManager<Domain.Entities.User> userManager, UserProvider<Domain.Entities.User> userProvider) : IActionHandler<EditProfileAction, EditProfileRequest, EditProfileDTO>
{
    public async Task<EditProfileDTO> Handle(EditProfileRequest request, CancellationToken token = default)
    {
        var user = await userProvider.GetCurrentOrFailAsync();
        var form = request.Form;

        if (form.FullName is not null)
        {
            user.FullName = form.FullName;
        }

        var result = await userManager.UpdateAsync(user);
        result.ThrowIfFailed();

        return new EditProfileDTO(user.FullName, user.Email!);
    }
}
