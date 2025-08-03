using Microsoft.AspNetCore.Mvc;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Authorization;

public class AuthorizeActionAttribute<TAction> : TypeFilterAttribute<AuthorizeActionFilter<TAction>>
    where TAction : IAction
{ }