using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Configuration;

namespace Saorsa.Pythagoras.Domain.Auth;

public class DefaultPythagorasAuthorizationManager : IPythagorasAuthorizationManager
{
    private readonly ILogger<DefaultPythagorasAuthorizationManager> _logger;
    public string DefaultScheme => AuthorizationConfiguration.DefaultMode.ToString();

    public string GetSchemeName(PythagorasAuthorizationMode mode)
    {
        return mode.ToString();
    }
    
    public PythagorasAuthorizationConfiguration AuthorizationConfiguration { get; }
    
    public DefaultPythagorasAuthorizationManager(
        PythagorasConfiguration config,
        ILogger<DefaultPythagorasAuthorizationManager> logger)
    {
        _logger = logger;
        AuthorizationConfiguration = config.Authorization;
    }
}
