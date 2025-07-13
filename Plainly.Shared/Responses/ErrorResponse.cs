using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Responses;

public class ErrorResponse : IResponse, IConvertToActionResult
{
    public ErrorResponse() : this(StatusCodes.Status500InternalServerError) { }

    public ErrorResponse(int status)
    {
        if (status is not (>= 400 and <= 599))
            throw new ArgumentException("Error response status code must be in the 400s or 500s");

        _Status = status;
    }

    private readonly int _Status;


    public bool Success => false;
    public required string Message { get; init; }
    public required string TraceId { get; init; }

    public IActionResult Convert()
    {
        return new ObjectResult(this) { StatusCode = _Status };
    }
}




