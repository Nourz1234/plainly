using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Infrastructure.Actions;
using Plainly.Api.Infrastructure.Authorization;
using Plainly.Shared;
using Plainly.Shared.Actions.User.EditProfile;
using Plainly.Shared.Actions.User.ViewProfile;
using Plainly.Shared.Responses;

namespace Plainly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ActionDispatcher actionDispatcher) : ControllerBase
{
    [AuthorizeAction<ViewProfileAction>]
    [HttpGet("Profile")]
    public async Task<SuccessResponse<ViewProfileDTO>> ViewProfile()
    {
        var result = await actionDispatcher.Dispatch<ViewProfileAction, ViewProfileRequest, ViewProfileDTO>(new ViewProfileRequest());

        return new SuccessResponse<ViewProfileDTO> { Message = Messages.Success, Data = result };
    }

    [AuthorizeAction<EditProfileAction>]
    [HttpPost("Profile")]
    public async Task<SuccessResponse> EditProfile(EditProfileFrom form)
    {
        await actionDispatcher.Dispatch<EditProfileAction, EditProfileRequest>(new EditProfileRequest(form));

        return new SuccessResponse { Message = Messages.Success };
    }
}