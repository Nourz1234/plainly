using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Plainly.Shared.Builders;

namespace Plainly.Shared.Responses;

public record ErrorResponse() : BaseResponse()
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ErrorDetail[]? Errors { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TraceId { get; init; }


    public static ErrorResponseBuilder InternalServerError()
        => new(StatusCodes.Status500InternalServerError, Messages.InternalServerError);

    public static ErrorResponseBuilder BadRequest()
        => new(StatusCodes.Status400BadRequest, Messages.BadRequest);

    public static ErrorResponseBuilder Unauthorized()
        => new(StatusCodes.Status401Unauthorized, Messages.Unauthorized);

    public static ErrorResponseBuilder Forbidden()
        => new(StatusCodes.Status403Forbidden, Messages.Forbidden);

    public static ErrorResponseBuilder NotFound()
        => new(StatusCodes.Status404NotFound, Messages.NotFound);

    public static ErrorResponseBuilder ValidationError()
        => new(StatusCodes.Status422UnprocessableEntity, Messages.ValidationError);
}