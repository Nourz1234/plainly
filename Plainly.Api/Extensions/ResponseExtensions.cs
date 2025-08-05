using Microsoft.AspNetCore.Mvc;
using Plainly.Shared.Responses;

namespace Plainly.Api.Extensions;

public static class ResponseExtensions
{
    public static IActionResult ToActionResult<T>(this T response) where T : BaseResponse
    {
        return new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        };
    }
}