using Plainly.Api.Models;

namespace Plainly.Api.Interfaces;

public interface IAuthTokenService
{
    string GenerateToken(User user);
}