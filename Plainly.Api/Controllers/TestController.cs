using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Exceptions;
using Plainly.Shared.Responses;

namespace Plainly.Api.Controllers;

[ExcludeFromCodeCoverage]
[Route("api/[controller]")]
[ApiController]
public class TestController(IHostEnvironment environment) : ControllerBase
{
    [HttpGet("Exception")]
    public SuccessResponse GetException()
    {
        if (environment.IsProduction()) throw new NotFoundException();
        throw new Exception();
    }

    [HttpGet("InternalServerError")]
    public SuccessResponse GetInternalServerError()
    {
        if (environment.IsProduction()) throw new NotFoundException();
        throw new InternalServerErrorException();
    }

    [HttpGet("NotFound")]
    public SuccessResponse GetNotFound()
    {
        if (environment.IsProduction()) throw new NotFoundException();
        throw new NotFoundException();
    }

    [HttpGet("Unauthorized")]
    public SuccessResponse GetUnauthorized()
    {
        if (environment.IsProduction()) throw new NotFoundException();
        throw new UnauthorizedException();
    }

    [HttpGet("Forbidden")]
    public SuccessResponse GetForbidden()
    {
        if (environment.IsProduction()) throw new NotFoundException();
        throw new ForbiddenException();
    }

    [HttpGet("BadRequest")]
    public SuccessResponse GetBadRequest()
    {
        if (environment.IsProduction()) throw new NotFoundException();
        throw new BadRequestException();
    }
}