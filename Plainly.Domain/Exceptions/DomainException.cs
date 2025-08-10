using Plainly.Domain.Extensions;
using Plainly.Shared.Extensions;
using Plainly.Shared.Responses;

namespace Plainly.Domain.Exceptions;

public class DomainException(DomainErrorCode errorCode, ErrorDetail[]? errors = null) : Exception
{
    public DomainErrorType Type { get; } = errorCode.GetErrorType();
    public string ErrorCode { get; } = errorCode.ToString();
    public string Description { get; } = errorCode.GetDescription();
    public ErrorDetail[]? Errors { get; } = errors;
}
