using Microsoft.AspNetCore.Mvc;
using Plainly.Api.Infrastructure.Authorization.Filters;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Authorization.Attributes;

public class AuthorizeFor<TAction> : TypeFilterAttribute
    where TAction : IAction
{
    public AuthorizeFor() : base(typeof(AuthorizeForFilter<TAction>)) { }
}