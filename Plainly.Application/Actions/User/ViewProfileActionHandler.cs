using Plainly.Api.Extensions;
using Plainly.Api.Infrastructure.Identity;
using Plainly.Shared.Actions.User.ViewProfile;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Actions.User;

public class ViewProfileActionHandler(UserProvider<Domain.Entities.User> userProvider)
    : IActionHandler<ViewProfileAction, ViewProfileRequest, ViewProfileDTO>
{
    public async Task<ViewProfileDTO> Handle(ViewProfileRequest request, CancellationToken token = default)
    {
        var user = await userProvider.GetCurrentOrFailAsync();

        return new ViewProfileDTO(user.FullName, user.Email!);
    }
}
