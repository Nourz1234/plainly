using System.Security.Claims;

namespace Plainly.Frontend.Services;

public class CurrentUserService
{

    public event Action<ClaimsPrincipal>? UserChanged;

    private ClaimsPrincipal _CurrentUser = new();
    public ClaimsPrincipal CurrentUser
    {
        get { return _CurrentUser ?? new(); }
        set
        {
            if (_CurrentUser != value)
            {
                _CurrentUser = value;
                UserChanged?.Invoke(_CurrentUser);
            }
        }
    }
}