using System.Security.Claims;
using Saorsa.Pythagoras.Domain.Model;

namespace Saorsa.Pythagoras.Domain.Auth;

public interface IPythagorasSessionManager
{
    void SignIn(ClaimsIdentity identity);
    bool IsLoggedIn { get; }
    bool IsSuperAdmin { get; }
    IdentityContext? GetLoggedInUser();
}
