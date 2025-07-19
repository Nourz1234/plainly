using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Plainly.Shared.Abstractions;

public abstract class BaseResponse(int status) : IConvertToActionResult
{
    public abstract bool Success { get; }

    public required abstract string Message { get; init; }

    public int GetStatusCode() => status;
    public IActionResult Convert()
    {
        return new ObjectResult(this) { StatusCode = status };
    }
}