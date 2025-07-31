using Microsoft.AspNetCore.Http;
using Plainly.Shared.Builders;

namespace Plainly.Shared.Responses;

public record SuccessResponse() : BaseResponse()
{
    public static SuccessResponseBuilder Ok()
        => new(StatusCodes.Status200OK, Messages.Success);

    public static SuccessResponseBuilder Created()
        => new(StatusCodes.Status201Created, Messages.Success);
}

public record SuccessResponse<T>() : SuccessResponse()
{
    public required T Data { get; init; }
}
