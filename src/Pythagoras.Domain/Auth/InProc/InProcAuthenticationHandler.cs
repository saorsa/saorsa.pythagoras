using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Saorsa.Pythagoras.InterProcess;

namespace Saorsa.Pythagoras.Domain.Auth.InProc;

public class InProcAuthenticationHandler : IAuthenticationHandler
{
    public IProcessRunner ProcessRunner { get; }
    public IPythagorasAuthorizationManager AuthorizationManager { get; }
    public IPythagorasIdentityProvider IdentityProvider { get; }

    public InProcAuthenticationHandler(
        IProcessRunner processRunner,
        IPythagorasAuthorizationManager authorizationManager,
        IPythagorasIdentityProvider identityProvider)
    {
        ProcessRunner = processRunner;
        AuthorizationManager = authorizationManager;
        IdentityProvider = identityProvider;
    }
    
    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        return Task.CompletedTask;
    }
    
    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        var userGroupsResult = await ProcessRunner.RunCommandProcessAsync("groups $USER");
        
        var machine = Environment.MachineName;
        var userName = $"{Environment.UserDomainName}/{Environment.UserName}";
        var identity = new ClaimsIdentity(new []
        {
            new Claim("user", userName, null, machine, machine)
        }, AuthorizationManager.DefaultScheme, "user", "group");


        if (userGroupsResult.IsSuccess && userGroupsResult.StandardOutput.Length > 0)
        {
            var groups = userGroupsResult.StandardOutput.Split(' ');

            foreach (var g in groups)
            {
                identity.AddClaim(new Claim("group",
                    g.Replace(Environment.NewLine, string.Empty), 
                    null, 
                    machine, 
                    machine));
            }
        }
        
        var principal = new ClaimsPrincipal(identity);

        IdentityProvider.SignIn(principal, identity.NameClaimType, identity.RoleClaimType);
        return
            AuthenticateResult.Success(
                new AuthenticationTicket(principal, AuthorizationManager.DefaultScheme));
    }

    public Task ChallengeAsync(AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }

    public Task ForbidAsync(AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }
}
