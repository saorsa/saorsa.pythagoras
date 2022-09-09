using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Auth.InProc;
using Saorsa.Pythagoras.Domain.Configuration;

namespace Saorsa.Pythagoras.Domain.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPythagorasAuthServices(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPythagorasAuthorizationManager, DefaultPythagorasAuthorizationManager>();
        serviceCollection.AddScoped<IPythagorasIdentityProvider, DefaultPythagorasIdentityProvider>();
        
        var provider = serviceCollection.BuildServiceProvider();
        var scope = provider.CreateScope();
        var authManager = scope.ServiceProvider.GetRequiredService<IPythagorasAuthorizationManager>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<PythagorasAuthenticationConfiguration>>();
        logger.LogInformation(
            "Using authorization with default scheme '{PythagorasAuthMode}'...",
            authManager.AuthenticationConfiguration.DefaultMode);
        
        serviceCollection
            .AddAuthentication((options) =>
            {
                options.DefaultScheme = authManager.DefaultAuthenticationScheme;
                
                options.AddScheme(authManager.GetAuthenticationSchemeName(PythagorasAuthenticationMode.InProc), builder =>
                {
                    builder.HandlerType = typeof(InProcAuthenticationHandler);
                });
            });
        
        scope.Dispose();
        provider.Dispose();
        return serviceCollection;
    }
}
