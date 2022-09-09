using System.Security.Claims;
using Saorsa.Pythagoras.Domain.Model;

namespace Saorsa.Pythagoras.Domain.Auth;

public interface IPythagorasIdentityProvider
{
    void SignIn(ClaimsPrincipal claimsId, string userClaimType = "user", string roleClaimType = "group");
    bool IsLoggedIn { get; }
    bool IsSuperAdmin { get; }
    IdentityContext? GetLoggedInUser();
}
