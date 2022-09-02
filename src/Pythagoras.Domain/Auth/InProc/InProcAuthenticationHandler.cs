using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Saorsa.Pythagoras.Domain.Auth.InProc;

public class InProcAuthenticationHandler : IAuthenticationHandler
{
    public IPythagorasIdentityProvider PythagorasIdentityProvider { get; }

    public InProcAuthenticationHandler(IPythagorasIdentityProvider pythagorasIdentityProvider)
    {
        PythagorasIdentityProvider = pythagorasIdentityProvider;
    }
    
    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        return Task.CompletedTask;
    }
    
    public Task<AuthenticateResult> AuthenticateAsync()
    {
        var machine = Environment.MachineName;
        var userName = $"{Environment.UserDomainName}/{Environment.UserName}";
        var identity = new ClaimsIdentity(new []
        {
            new Claim("user", userName, "", machine, machine)
        }, "INPROC", "user", "group");
        var principal = new ClaimsPrincipal(identity);

        PythagorasIdentityProvider.SignIn(principal, identity.NameClaimType, identity.RoleClaimType);
        return Task.FromResult(
            AuthenticateResult.Success(
                new AuthenticationTicket(principal, "INPROC"))
        );
    }

    public Task ChallengeAsync(AuthenticationProperties properties)
    {
        return Task.CompletedTask;
    }

    public Task ForbidAsync(AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }
}
