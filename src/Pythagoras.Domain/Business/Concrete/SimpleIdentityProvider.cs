using Saorsa.Pythagoras.Domain.Model;

namespace Saorsa.Pythagoras.Domain.Business.Concrete;

public class SimpleIdentityProvider : IIdentityProvider
{
    private IdentityContext? _identityContext;

    public bool IsLoggedIn => _identityContext != null;
    
    public IdentityContext? GetLoggedInUser()
    {
        return _identityContext;
    }

    public void SetLoggedInUser(IdentityContext? user)
    {
        _identityContext = user;
    }
    
    public void SetLoggedInUser(string user, IEnumerable<string>? groups)
    {
        _identityContext = new IdentityContext(user, groups);
    }
}
