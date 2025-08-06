namespace Plainly.Domain.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class ErrorTypeAttribute(DomainErrorType errorType) : Attribute
{
    public DomainErrorType ErrorType { get; } = errorType;
}