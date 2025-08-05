using Plainly.Application.Extensions;
using Plainly.Application.Interface;
using Plainly.Shared.Actions.User.ViewProfile;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions.User;

public class ViewProfileActionHandler(ICurrentUserProvider userProvider)
    : IActionHandler<ViewProfileAction, ViewProfileRequest, ViewProfileDTO>
{
    public async Task<ViewProfileDTO> Handle(ViewProfileRequest request, CancellationToken token = default)
    {
        var user = await userProvider.GetCurrentUserOrFailAsync();

        return new ViewProfileDTO(user.FullName, user.Email!);
    }
}
