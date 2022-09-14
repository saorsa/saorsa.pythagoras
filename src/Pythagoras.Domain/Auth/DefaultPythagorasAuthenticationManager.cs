using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Business;
using Saorsa.Pythagoras.Domain.Configuration;

namespace Saorsa.Pythagoras.Domain.Auth;

public class DefaultPythagorasAuthenticationManager : IPythagorasAuthenticationManager
{
    public string? DefaultAuthenticationScheme => AuthenticationConfiguration.DefaultAuthenticationScheme;
    public string InProcAuthenticationScheme => GetAuthenticationSchemeName(PythagorasAuthenticationMode.InProc);
    public string OidcAuthenticationScheme => GetAuthenticationSchemeName(PythagorasAuthenticationMode.Oidc);
    
    public PythagorasAuthenticationConfiguration AuthenticationConfiguration { get; }
    
    public DefaultPythagorasAuthenticationManager(
        PythagorasConfiguration config,
        ILogger<DefaultPythagorasAuthenticationManager> logger)
    {
        AuthenticationConfiguration = config.Authentication;
    }
    
    
    public string GetAuthenticationSchemeName(PythagorasAuthenticationMode mode)
    {
        switch (mode)
        {
            case PythagorasAuthenticationMode.InProc:
                return AuthenticationConfiguration.InProc.AuthenticationScheme;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    public PythagorasAuthenticationMode? GetAuthenticationMode(string authenticationSchemeName)
    {
        if (AuthenticationConfiguration.InProc.AuthenticationScheme.Equals(authenticationSchemeName))
        {
            return PythagorasAuthenticationMode.InProc;
        }

        return default;
    }
    
    public bool IsAuthenticationSchemeEnabled(string authenticationSchemeName)
    {
        return GetAuthenticationMode(authenticationSchemeName).HasValue;
    }

    public string GetUserClaimType(string authenticationSchemeName)
    {
        var mode = GetAuthenticationMode(authenticationSchemeName);

        if (!mode.HasValue)
        {
            throw new PythagorasException(
                ErrorCodes.Auth.Forbidden,
                "Invalid authentication scheme");
        }
        
        return mode switch
        {
            PythagorasAuthenticationMode.InProc => AuthenticationConfiguration.InProc.UserClaimType,
            _ => throw new NotImplementedException()
        };
    }

    public string GetRoleClaimType(string authenticationSchemeName)
    {
        var mode = GetAuthenticationMode(authenticationSchemeName);

        if (!mode.HasValue)
        {
            throw new PythagorasException(
                ErrorCodes.Auth.Forbidden,
                "Invalid authentication scheme");
        }
        
        return mode switch
        {
            PythagorasAuthenticationMode.InProc => AuthenticationConfiguration.InProc.RoleClaimType,
            _ => throw new NotImplementedException()
        };
    }

    public string GetSuperAdminUser(string authenticationSchemeName)
    {
        var mode = GetAuthenticationMode(authenticationSchemeName);

        if (!mode.HasValue)
        {
            throw new PythagorasException(
                ErrorCodes.Auth.Forbidden,
                "Invalid authentication scheme");
        }

        return mode switch
        {
            PythagorasAuthenticationMode.InProc => AuthenticationConfiguration.InProc.SuperAdminUser,
            _ => throw new NotImplementedException()
        };
    }
}
