using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Infrastructure.Actions;
using Plainly.Api.Infrastructure.Authorization;
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

        return SuccessResponse.Ok().Build(result);
    }

    [AuthorizeAction<EditProfileAction>]
    [HttpPatch("Profile")]
    public async Task<SuccessResponse<EditProfileDTO>> EditProfile(EditProfileFrom form)
    {
        var result = await actionDispatcher.Dispatch<EditProfileAction, EditProfileRequest, EditProfileDTO>(new EditProfileRequest(form));

        return SuccessResponse.Ok().Build(result);
    }
}