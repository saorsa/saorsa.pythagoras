using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Configuration;

namespace Saorsa.Pythagoras.Domain.Auth;

public class DefaultPythagorasAuthorizationManager : IPythagorasAuthorizationManager
{
    private readonly ILogger<DefaultPythagorasAuthorizationManager> _logger;

    public string? DefaultAuthenticationScheme => AuthenticationConfiguration.DefaultAuthenticationScheme;

    public string GetAuthenticationSchemeName(PythagorasAuthenticationMode mode)
    {
        return mode.ToString();
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
    
    public PythagorasAuthenticationConfiguration AuthenticationConfiguration { get; }
    
    public DefaultPythagorasAuthorizationManager(
        PythagorasConfiguration config,
        ILogger<DefaultPythagorasAuthorizationManager> logger)
    {
        _logger = logger;
        AuthenticationConfiguration = config.Authentication;
    }
}
