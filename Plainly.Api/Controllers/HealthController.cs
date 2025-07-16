using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Infrastructure.Action;
using Plainly.Api.Infrastructure.Authorization;
using Plainly.Shared;
using Plainly.Shared.Actions.Heath.GetHealth;
using Plainly.Shared.Responses;

namespace Plainly.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class HealthController(ActionDispatcher actionDispatcher) : ControllerBase
{
    [AuthorizeFor<GetHealthAction>]
    [HttpGet]
    public async Task<SuccessResponse<GetHealthDTO>> Get()
    {
        var result = await actionDispatcher.Dispatch<GetHealthAction, GetHealthRequest, GetHealthDTO>(new GetHealthRequest());

        return new SuccessResponse<GetHealthDTO>
        {
            Message = Messages.Success,
            Data = result
        };
    }
}
