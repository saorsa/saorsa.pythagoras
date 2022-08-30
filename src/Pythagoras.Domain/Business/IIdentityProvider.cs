using Saorsa.Pythagoras.Domain.Model;

namespace Saorsa.Pythagoras.Domain.Business;

public interface IIdentityProvider
{
    bool IsLoggedIn { get; }
    IdentityContext? GetLoggedInUser();
}
