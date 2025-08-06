using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Exceptions;
using Plainly.Domain;
using Plainly.Domain.Exceptions;
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
        if (environment.IsProduction()) throw new ApiException(ApiErrorCode.EndpointNotFound);
        throw new Exception();
    }

    [HttpGet("InternalServerError")]
    public SuccessResponse GetInternalServerError()
    {
        if (environment.IsProduction()) throw new ApiException(ApiErrorCode.EndpointNotFound);
        throw DomainException.FromErrorCode(DomainErrorCode.InternalError);
    }

    [HttpGet("NotFound")]
    public SuccessResponse GetNotFound()
    {
        if (environment.IsProduction()) throw new ApiException(ApiErrorCode.EndpointNotFound);
        throw DomainException.FromErrorCode(DomainErrorCode.NotFound);
    }

    [HttpGet("Unauthorized")]
    public SuccessResponse GetUnauthorized()
    {
        if (environment.IsProduction()) throw new ApiException(ApiErrorCode.EndpointNotFound);
        throw DomainException.FromErrorCode(DomainErrorCode.Unauthorized);
    }

    [HttpGet("Forbidden")]
    public SuccessResponse GetForbidden()
    {
        if (environment.IsProduction()) throw new ApiException(ApiErrorCode.EndpointNotFound);
        throw DomainException.FromErrorCode(DomainErrorCode.Forbidden);
    }

    [HttpGet("BadRequest")]
    public SuccessResponse GetBadRequest()
    {
        if (environment.IsProduction()) throw new ApiException(ApiErrorCode.EndpointNotFound);
        throw DomainException.FromErrorCode(DomainErrorCode.InvalidOperation);
    }
}