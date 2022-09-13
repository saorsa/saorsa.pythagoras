using Saorsa.Pythagoras.Domain.Configuration;

namespace Saorsa.Pythagoras.Domain.Auth;

public interface IPythagorasAuthenticationManager
{
    string? DefaultAuthenticationScheme { get; }
    string InProcAuthenticationScheme { get; }
    string OidcAuthenticationScheme { get; }
    string GetAuthenticationSchemeName(PythagorasAuthenticationMode mode);
    bool IsAuthenticationSchemeEnabled(string authenticationSchemeName);
    public PythagorasAuthenticationConfiguration AuthenticationConfiguration { get; }

    string GetUserClaimType(string authenticationSchemeName);
    string GetRoleClaimType(string authenticationSchemeName);
    string GetSuperAdminUser(string authenticationSchemeName);
}
