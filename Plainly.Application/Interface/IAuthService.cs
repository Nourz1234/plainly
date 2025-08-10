using Plainly.Domain.Interfaces;

namespace Plainly.Application.Interface;


public interface IAuthService
{
    Task CheckPasswordAsync(IUser user, string password);
}