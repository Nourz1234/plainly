using Plainly.Domain.Interfaces;

namespace Plainly.Application.Interface;


public interface IAuthService
{
    Task VerifyPasswordAsync(IUser user, string password);
}