using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Saorsa.Pythagoras.Domain.Configuration;
using Saorsa.Pythagoras.Domain.Model;

namespace Saorsa.Pythagoras.Domain.Auth;

public class DefaultPythagorasIdentityProvider : IPythagorasIdentityProvider
{
    public ILogger<DefaultPythagorasIdentityProvider> Logger { get; }
    private readonly PythagorasConfiguration _pythagorasConfiguration;
    private IdentityContext? _identityContext;
    private ClaimsPrincipal? _claimsPrincipal;

    public bool IsLoggedIn => _identityContext != null;

    public bool IsSuperAdmin => IsLoggedIn && _identityContext != null &&
                                _identityContext.User.Equals(SuperAdminUser,
                                    StringComparison.InvariantCultureIgnoreCase);

    public string SuperAdminUser => _pythagorasConfiguration.SuperAdminUser;

    public DefaultPythagorasIdentityProvider(
        IOptions<PythagorasConfiguration> opts,
        ILogger<DefaultPythagorasIdentityProvider> logger)
    {
        Logger = logger;
        _pythagorasConfiguration = opts.Value;
    }
    
    public IdentityContext? GetLoggedInUser()
    {
        return _identityContext;
    }

    public void SignIn(ClaimsPrincipal claimsId, string userClaimType = "user", string roleClaimType = "group")
    {
        var userClaim = claimsId.Claims
            .SingleOrDefault(c => c.Type.Equals(userClaimType, StringComparison.InvariantCultureIgnoreCase));

        if (userClaim == null)
        {
            throw new ArgumentException(
                $"The specified claims principal does not contain a claim of type '{userClaimType}'.");
        }

        var groupClaims = claimsId.Claims
            .Where(c => c.Type.Equals(roleClaimType, StringComparison.InvariantCultureIgnoreCase))
            .Select(c => c.Value);

        Logger.LogWarning("Signing user {User} (Groups: {UserGroups})", userClaim.Value,
            string.Join(", ", groupClaims));
        _claimsPrincipal = claimsId;
        _identityContext = new IdentityContext(userClaim.Value, groupClaims);
    }

    public void SignOut()
    {
        Logger.LogWarning("Signing out");
        _claimsPrincipal = null;
        _identityContext = null;
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
