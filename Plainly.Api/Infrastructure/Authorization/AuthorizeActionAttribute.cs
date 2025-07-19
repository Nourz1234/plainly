using Microsoft.AspNetCore.Mvc;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Authorization;

public class AuthorizeActionAttribute<TAction> : TypeFilterAttribute<AuthorizeActionFilter<TAction>>
    where TAction : IAction
{ }