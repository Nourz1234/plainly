using System.ComponentModel;
using Plainly.Api.Attributes;

namespace Plainly.Api;

public enum ApiErrorCode
{
    [StatusCode(StatusCodes.Status404NotFound)]
    [Description(ApiMessages.EndpointNotFound)]
    EndpointNotFound,
}