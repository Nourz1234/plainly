using Microsoft.AspNetCore.Http;
using Plainly.Shared.Abstract;

namespace Plainly.Shared.Responses;

public class SuccessResponse : BaseResponse
{
    public SuccessResponse() : base(StatusCodes.Status200OK) { }

    public SuccessResponse(int status) : base(status)
    {
        if (status is not (>= 200 and <= 299))
            throw new ArgumentException("Success response status code must be in the 200s");
    }

    public override bool Success => true;
    public required override string Message { get; init; }
}

public class SuccessResponse<T> : SuccessResponse
{
    public SuccessResponse() : base() { }
    public SuccessResponse(int status) : base(status) { }

    public required T Data { get; init; }
}
