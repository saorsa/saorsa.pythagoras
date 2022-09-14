using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.InterProcess;

namespace Saorsa.Pythagoras.Domain.Auth.InProc;

public class InProcAuthenticationHandler : IAuthenticationHandler
{
    public ILogger<InProcAuthenticationHandler> Logger { get; }
    public IProcessRunner ProcessRunner { get; }
    public IPythagorasAuthenticationManager AuthenticationManager { get; }
    public IPythagorasSessionManager SessionManager { get; }

    public InProcAuthenticationHandler(
        ILogger<InProcAuthenticationHandler> logger,
        IProcessRunner processRunner,
        IPythagorasAuthenticationManager authenticationManager,
        IPythagorasSessionManager sessionManager)
    {
        Logger = logger;
        ProcessRunner = processRunner;
        AuthenticationManager = authenticationManager;
        SessionManager = sessionManager;
    }
    
    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        return Task.CompletedTask;
    }
    
    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        var config = AuthenticationManager.AuthenticationConfiguration.InProc;
        var machine = Environment.MachineName;
        var userName = $"{Environment.UserDomainName}/{Environment.UserName}";
        var identity = new ClaimsIdentity(new []
        {
            new Claim(
                config.UserClaimType, 
                userName, null, machine, machine)
        },
            AuthenticationManager.InProcAuthenticationScheme, 
            config.UserClaimType, 
            config.RoleClaimType);

        await AddProcessUserGroups(identity, machine);
        
        var principal = new ClaimsPrincipal(identity);

        SessionManager.SignIn(identity);

        return
            AuthenticateResult.Success(
                new AuthenticationTicket(principal, AuthenticationManager.InProcAuthenticationScheme));
    }

    public async Task AddProcessUserGroups(
        ClaimsIdentity identity, string issuer)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var config = AuthenticationManager.AuthenticationConfiguration.InProc;
            var userGroupsResult = await ProcessRunner.RunBashCommandAsync("groups $USER");
            if (userGroupsResult.IsSuccess && userGroupsResult.StandardOutput.Length > 0)
            {
                var groups = userGroupsResult.StandardOutput
                    .Replace(":", string.Empty)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Distinct();

                foreach (var g in groups)
                {
                    identity.AddClaim(new Claim(config.RoleClaimType,
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
                    $"Error fetching user groups ({userGroupsResult.ExitCode??-1}): " +
                    $"{userGroupsResult.ErrorOutput}");
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
