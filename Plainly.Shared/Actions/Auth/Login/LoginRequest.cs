using Plainly.Shared.Interfaces;

namespace Plainly.Shared.Actions.Auth.Login;

public record LoginRequest(LoginForm LoginForm) : IActionRequest<LoginDTO>;
