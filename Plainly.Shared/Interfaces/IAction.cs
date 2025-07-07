using FluentValidation;

namespace Plainly.Shared.Interfaces;


public interface IAction<TRequest, TResponse>
    where TRequest : class
    where TResponse : class
{
    public string Name { get; }
    public string Claim { get; }
}