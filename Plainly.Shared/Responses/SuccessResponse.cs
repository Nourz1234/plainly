using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Responses;

public class SuccessResponse : IResponse, IConvertToActionResult
{
    public SuccessResponse() : this(StatusCodes.Status200OK) { }

    public SuccessResponse(int status)
    {
        if (status is not (>= 200 and <= 299))
            throw new ArgumentException("Success response status code must be in the 200s");

        _Status = status;
    }

    private readonly int _Status;

    public bool Success => true;
    public required string Message { get; init; }

    public IActionResult Convert()
    {
        return new OkObjectResult(this) { StatusCode = _Status };
    }
}

public class SuccessResponse<T> : SuccessResponse
{
    public SuccessResponse() : base() { }
    public SuccessResponse(int status) : base(status) { }

    public required T Data { get; init; }
}




