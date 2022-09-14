using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Model;

namespace Saorsa.Pythagoras.Domain.Auth;

public class DefaultPythagorasSessionManager : IPythagorasSessionManager
{
    public IPythagorasAuthenticationManager AuthenticationManager { get; }
    public ILogger<DefaultPythagorasSessionManager> Logger { get; }
    private readonly object SessionLock = new { };
    private IdentitySession? _session;
    private ClaimsIdentity? _claimsId;

    public bool IsLoggedIn => _session != null;

    public bool IsSuperAdmin
    {
        get
        {
            lock (SessionLock)
            {
                if (!IsLoggedIn || _session == null || _claimsId == null) return false;
                var superAdmin = AuthenticationManager.GetSuperAdminUser(_claimsId.AuthenticationType!);
                return _session.User.Equals(superAdmin);
            }
        }
    }

    public DefaultPythagorasSessionManager(
        IPythagorasAuthenticationManager authenticationManager,
        ILogger<DefaultPythagorasSessionManager> logger)
    {
        AuthenticationManager = authenticationManager;
        Logger = logger;
    }
    
    public IdentitySession? GetLoggedInUser()
    {
        return _session;
    }

    public void SignIn(ClaimsIdentity identity)
    {
        var authenticationType = identity.AuthenticationType ??
                                 throw new ArgumentNullException(nameof(identity.AuthenticationType));
        var userClaimType = AuthenticationManager.GetUserClaimType(authenticationType);
        var roleClaimType = AuthenticationManager.GetRoleClaimType(authenticationType);

        var userClaim = identity.Claims
            .SingleOrDefault(c => c.Type.Equals(userClaimType, StringComparison.InvariantCultureIgnoreCase));

        if (userClaim == null)
        {
            throw new ArgumentException(
                $"The specified claims principal does not contain a claim of type '{userClaimType}'.");
        }

        var groupClaims = identity.Claims
            .Where(c => c.Type.Equals(roleClaimType, StringComparison.InvariantCultureIgnoreCase))
            .Select(c => c.Value)
            .ToList();

        Logger.LogWarning("Signing user {User} (Groups: {UserGroups})", userClaim.Value,
            string.Join(", ", groupClaims));

        lock (SessionLock)
        {
            _claimsId = identity;
            _session = new IdentitySession(
                authenticationType,
                userClaim.Value,
                groupClaims);
        }
    }

    public void SignOut()
    {
        if (IsLoggedIn)
        {
            Logger.LogWarning("Signing out {User}...", _session!.User);
            lock (SessionLock)
            {
                _claimsId = null;
                _session = null;
            }
        }
        else
        {
            Logger.LogWarning("Calling sign-out without an active session");
        }
    }
}
