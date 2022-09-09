using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Configuration;
using Saorsa.Pythagoras.InterProcess;
using Serilog;

namespace Saorsa.Pythagoras.Domain.Auth.InProc;

public class InProcAuthenticationHandler : IAuthenticationHandler
{
    public ILogger<InProcAuthenticationHandler> Logger { get; }
    public IProcessRunner ProcessRunner { get; }
    public IPythagorasAuthorizationManager AuthorizationManager { get; }
    public IPythagorasIdentityProvider IdentityProvider { get; }

    public InProcAuthenticationHandler(
        ILogger<InProcAuthenticationHandler> logger,
        IProcessRunner processRunner,
        IPythagorasAuthorizationManager authorizationManager,
        IPythagorasIdentityProvider identityProvider)
    {
        Logger = logger;
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
        var machine = Environment.MachineName;
        var userName = $"{Environment.UserDomainName}/{Environment.UserName}";
        var identity = new ClaimsIdentity(new []
        {
            new Claim(
                AuthorizationManager.AuthenticationConfiguration.UserClaimType, 
                userName, null, machine, machine)
        },
            AuthorizationManager.GetAuthenticationSchemeName(PythagorasAuthenticationMode.InProc), 
            AuthorizationManager.AuthenticationConfiguration.UserClaimType, 
            AuthorizationManager.AuthenticationConfiguration.RoleClaimType);

        await AddProcessUserGroups(identity, machine);
        
        var principal = new ClaimsPrincipal(identity);

        IdentityProvider.SignIn(principal, identity.NameClaimType, identity.RoleClaimType);

        return
            AuthenticateResult.Success(
                new AuthenticationTicket(principal, AuthorizationManager.DefaultAuthenticationScheme));
    }

    public async Task AddProcessUserGroups(
        ClaimsIdentity identity, string issuer)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var userGroupsResult = await ProcessRunner.RunBashCommandAsync("groups $USER");
            if (userGroupsResult.IsSuccess && userGroupsResult.StandardOutput.Length > 0)
            {
                var groups = userGroupsResult.StandardOutput
                    .Replace(":", string.Empty)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var g in groups)
                {
                    identity.AddClaim(new Claim(AuthorizationManager.AuthenticationConfiguration.RoleClaimType,
                        g.Replace(Environment.NewLine, string.Empty), 
                        null, 
                        issuer, 
                        issuer));
                }
            }
            else
            {
                Logger.LogError("Error fetching user groups: {ErrorOutput}", userGroupsResult.ErrorOutput);
                throw new ApplicationException(
                    $"Error fetching user groups ({userGroupsResult.ExitCode??-1}): {userGroupsResult.ErrorOutput}");
            }
        }
        else
        {
            Logger.LogCritical(
                "Operating system {OperatingSystem} is not supported yet",
                RuntimeInformation.OSDescription);
            throw new NotSupportedException(
                $"Operating system {RuntimeInformation.OSDescription} is not supported yet.");
        }
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
