using Plainly.Api.Extensions;
using Plainly.Domain.Extensions;
using Plainly.Shared.Extensions;

namespace Plainly.Api.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; }
    public string ErrorCode { get; }

    public ApiException(ApiErrorCode errorCode) : base(errorCode.GetDescription())
    {
        StatusCode = errorCode.GetStatusCode();
        ErrorCode = errorCode.ToString();
    }

    public ApiException(ApiErrorCode errorCode, Exception? innerException) : base(errorCode.GetDescription(), innerException)
    {
        StatusCode = errorCode.GetStatusCode();
        ErrorCode = errorCode.ToString();
    }
}