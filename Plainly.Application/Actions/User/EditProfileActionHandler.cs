using Plainly.Application.Extensions;
using Plainly.Application.Interface;
using Plainly.Application.Interface.Repositories;
using Plainly.Shared.Actions.User.EditProfile;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions.User;

public class EditProfileActionHandler(IUserRepository userRepository, ICurrentUserProvider userProvider, IJwtService jwtService) : IActionHandler<EditProfileAction, EditProfileRequest, EditProfileDTO>
{
    public async Task<EditProfileDTO> Handle(EditProfileRequest request, CancellationToken token = default)
    {
        var user = await userProvider.GetCurrentUserOrFailAsync();
        var form = request.Form;

        if (form.FullName is not null)
        {
            user.FullName = form.FullName;
        }

        await userRepository.UpdateAsync(user);

        var userToken = await jwtService.GenerateToken(user);

        return new EditProfileDTO(userToken);
    }
}
