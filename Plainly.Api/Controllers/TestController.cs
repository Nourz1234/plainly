using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Plainly.Domain;
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
        if (environment.IsProduction()) throw DomainError.FromErrorCode(ErrorCode.NotFound);
        throw new Exception();
    }

    [HttpGet("InternalServerError")]
    public SuccessResponse GetInternalServerError()
    {
        if (environment.IsProduction()) throw DomainError.FromErrorCode(ErrorCode.NotFound);
        throw DomainError.FromErrorCode(ErrorCode.InternalError);
    }

    [HttpGet("NotFound")]
    public SuccessResponse GetNotFound()
    {
        if (environment.IsProduction()) throw DomainError.FromErrorCode(ErrorCode.NotFound);
        throw DomainError.FromErrorCode(ErrorCode.NotFound);
    }

    [HttpGet("Unauthorized")]
    public SuccessResponse GetUnauthorized()
    {
        if (environment.IsProduction()) throw DomainError.FromErrorCode(ErrorCode.NotFound);
        throw DomainError.FromErrorCode(ErrorCode.Unauthorized);
    }

    [HttpGet("Forbidden")]
    public SuccessResponse GetForbidden()
    {
        if (environment.IsProduction()) throw DomainError.FromErrorCode(ErrorCode.NotFound);
        throw DomainError.FromErrorCode(ErrorCode.Forbidden);
    }

    [HttpGet("BadRequest")]
    public SuccessResponse GetBadRequest()
    {
        if (environment.IsProduction()) throw DomainError.FromErrorCode(ErrorCode.NotFound);
        throw DomainError.FromErrorCode(ErrorCode.BadRequest);
    }
}