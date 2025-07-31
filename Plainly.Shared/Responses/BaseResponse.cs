using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Plainly.Shared.Responses;

public abstract record BaseResponse() : IConvertToActionResult
{
    public required bool Success { get; init; }
    public required int StatusCode { get; init; }
    public required string Message { get; init; }

    public IActionResult ToActionResult() => new ObjectResult(this) { StatusCode = StatusCode };

    IActionResult IConvertToActionResult.Convert() => ToActionResult();
}
