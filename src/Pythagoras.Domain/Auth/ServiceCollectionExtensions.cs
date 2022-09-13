using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Auth.InProc;
using Saorsa.Pythagoras.Domain.Configuration;

namespace Saorsa.Pythagoras.Domain.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPythagorasAuthServices(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPythagorasAuthenticationManager, DefaultPythagorasAuthenticationManager>();
        serviceCollection.AddScoped<IPythagorasSessionManager, DefaultPythagorasSessionManager>();
        
        var provider = serviceCollection.BuildServiceProvider();
        var scope = provider.CreateScope();
        var authManager = scope.ServiceProvider.GetRequiredService<IPythagorasAuthenticationManager>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<PythagorasAuthenticationConfiguration>>();
        var inProcSchemeName = authManager.InProcAuthenticationScheme;
        
        serviceCollection
            .AddAuthentication((options) =>
            {
                options.DefaultScheme = authManager.DefaultAuthenticationScheme;

                if (authManager.IsAuthenticationSchemeEnabled(inProcSchemeName))
                { 
                    logger.LogInformation(
                        "Adding authentication for scheme '{AuthenticationScheme}'...",
                        inProcSchemeName);
                    
                    options.DefaultScheme ??= inProcSchemeName;
                    options.AddScheme(inProcSchemeName, builder =>
                    {
                        builder.HandlerType = typeof(InProcAuthenticationHandler);
                    });
                }
            });
        
        scope.Dispose();
        provider.Dispose();
        return serviceCollection;
    }
}
