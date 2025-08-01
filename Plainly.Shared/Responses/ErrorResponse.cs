using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Plainly.Shared.Builders;
using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Responses;

public record ErrorResponse() : BaseResponse()
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ErrorDetail[]? Errors { get; init; }
    public required string TraceId { get; init; }

    public static ErrorResponseBuilder FromException<T>(T exception)
        where T : IHttpException
    {
        var builder = new ErrorResponseBuilder(exception.StatusCode, exception.Message);
        if (exception is IExceptionWithErrors { Errors: ErrorDetail[] errors })
        {
            builder.WithErrors(errors);
        }
        return builder;
    }

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