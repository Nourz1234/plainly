using Microsoft.AspNetCore.Mvc;
using Plainly.Shared.Interfaces;

namespace Plainly.Api.Infrastructure.Authorization;

public class AuthorizeForAttribute<TAction> : TypeFilterAttribute<AuthorizeForFilter<TAction>>
    where TAction : IAction
{ }