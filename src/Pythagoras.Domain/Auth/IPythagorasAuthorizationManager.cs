using Saorsa.Pythagoras.Domain.Configuration;

namespace Saorsa.Pythagoras.Domain.Auth;

public interface IPythagorasAuthorizationManager
{
    string DefaultScheme { get; }
    string GetSchemeName(PythagorasAuthorizationMode mode);
    public PythagorasAuthorizationConfiguration AuthorizationConfiguration { get; }
}
