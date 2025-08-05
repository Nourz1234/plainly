using Plainly.Shared.Extensions;
using Plainly.Shared.Responses;

namespace Plainly.Domain;

public class DomainError(string errorCode, string category, string description, ErrorDetail[]? errors = null) : Exception
{
    public string ErrorCode { get; } = errorCode;
    public string Category { get; } = category;
    public string Description { get; } = description;
    public ErrorDetail[]? Errors { get; } = errors;

    public static DomainError FromErrorCode(ErrorCode errorCode, ErrorDetail[]? errors = null)
        => new(errorCode.ToString(), errorCode.GetCategory(), errorCode.GetDescription(), errors);

    public static DomainError FromValidationErrors(params ErrorDetail[] errors)
        => FromErrorCode(Domain.ErrorCode.ValidationError, errors);
}