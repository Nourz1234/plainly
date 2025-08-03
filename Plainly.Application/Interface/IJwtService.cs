using Plainly.Domain.Interfaces;

namespace Plainly.Application.Interface;

public interface IJwtService
{
    Task<string> GenerateToken(IUser user);
}