using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Authorization;
using Plainly.Api.Builders;
using Plainly.Application.Actions;
using Plainly.Shared.Actions.Heath.GetHealth;
using Plainly.Shared.Responses;

namespace Plainly.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class HealthController(ActionDispatcher actionDispatcher) : ControllerBase
{
    [AuthorizeAction<GetHealthAction>]
    [HttpGet]
    public async Task<SuccessResponse<GetHealthDTO>> Get()
    {
        var result = await actionDispatcher.Dispatch<GetHealthAction, GetHealthRequest, GetHealthDTO>(new GetHealthRequest());

        return SuccessResponseBuilder.Ok().Build(result);
    }
}
