using Plainly.Shared.Responses;

namespace Plainly.Shared.Interfaces;

public interface IExceptionWithErrors
{
    ErrorDetail[]? Errors { get; }
}