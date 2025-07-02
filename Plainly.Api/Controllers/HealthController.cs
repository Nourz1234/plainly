using Microsoft.AspNetCore.Mvc;
using Plainly.Shared;
using Plainly.Shared.DTOs;
using Plainly.Shared.Responses;

namespace Plainly.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public SuccessResponse<HealthDTO> Get()
    {
        return new SuccessResponse<HealthDTO>
        {
            Message = Messages.Success,
            Data = new HealthDTO
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
            }
        };
    }
}
