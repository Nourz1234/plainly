using FluentValidation;
using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions;

// public abstract class Action<TRequest, TResponse, TValidator> : IAction<TRequest, TResponse, TValidator>
//     where TRequest : class
//     where TResponse : class
//     where TValidator : AbstractValidator<TRequest>
// {
//     public abstract string Name { get; }

//     public abstract string Claim { get; }
// }