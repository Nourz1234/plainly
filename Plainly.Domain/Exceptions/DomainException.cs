using Plainly.Domain.Extensions;
using Plainly.Shared.Extensions;
using Plainly.Shared.Responses;

namespace Plainly.Domain.Exceptions;

public class DomainException(DomainErrorType type, string errorCode, string description, ErrorDetail[]? errors = null) : Exception
{
    public DomainErrorType Type { get; } = type;
    public string ErrorCode { get; } = errorCode;
    public string Description { get; } = description;
    public ErrorDetail[]? Errors { get; } = errors;

    public static DomainException FromErrorCode(DomainErrorCode errorCode, ErrorDetail[]? errors = null)
        => new(errorCode.GetErrorType(), errorCode.ToString(), errorCode.GetDescription(), errors);

    public static DomainException FromValidationErrors(params ErrorDetail[] errors)
        => FromErrorCode(DomainErrorCode.ValidationError, errors);
}