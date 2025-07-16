using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Plainly.Shared.Abstract;

public abstract class BaseResponse(int _Status) : IConvertToActionResult
{
    public abstract bool Success { get; }

    public required abstract string Message { get; init; }

    public IActionResult Convert()
    {
        return new ObjectResult(this) { StatusCode = _Status };
    }
}