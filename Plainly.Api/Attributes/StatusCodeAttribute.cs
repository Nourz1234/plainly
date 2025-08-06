namespace Plainly.Api.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class StatusCodeAttribute(int statusCode) : Attribute
{
    public int StatusCode { get; } = statusCode;
}