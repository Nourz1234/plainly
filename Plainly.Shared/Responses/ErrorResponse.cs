using Microsoft.AspNetCore.Http;
using Plainly.Shared.Abstract;

namespace Plainly.Shared.Responses;

public class ErrorResponse : BaseResponse
{
    public ErrorResponse() : base(StatusCodes.Status500InternalServerError) { }

    public ErrorResponse(int status) : base(status)
    {
        if (status is not (>= 400 and <= 599))
            throw new ArgumentException("Error response status code must be in the 400s or 500s");
    }

    public override bool Success => false;
    public required override string Message { get; init; }
    public required string TraceId { get; init; }
}
