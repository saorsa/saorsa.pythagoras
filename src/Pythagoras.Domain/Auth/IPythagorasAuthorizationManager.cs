using Saorsa.Pythagoras.Domain.Configuration;

namespace Saorsa.Pythagoras.Domain.Auth;

public interface IPythagorasAuthorizationManager
{
    string? DefaultAuthenticationScheme { get; }
    string GetAuthenticationSchemeName(PythagorasAuthenticationMode mode);
    public PythagorasAuthenticationConfiguration AuthenticationConfiguration { get; }
}
