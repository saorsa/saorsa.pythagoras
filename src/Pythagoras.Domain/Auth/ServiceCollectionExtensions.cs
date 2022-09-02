using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Saorsa.Pythagoras.Domain.Auth.InProc;

namespace Saorsa.Pythagoras.Domain.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPythagorasAuthServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IPythagorasIdentityProvider, DefaultPythagorasIdentityProvider>();
        var pythagorasOpts = serviceCollection.ResolveRequiredService<IOptions<PythagorasOptions>>();
        var logger = serviceCollection.ResolveRequiredService<ILogger<PythagorasAuthorizationOptions>>(false);
        switch (pythagorasOpts.Value.Authorization.Mode)
        {
            case PythagorasAuthorizationMode.InProc:
                logger.LogWarning("Using INPROC authentication provider...");
                serviceCollection
                    .AddAuthentication((options) =>
                    {
                        options.DefaultScheme = "INPROC";
                        options.AddScheme("INPROC", builder =>
                        {
                            builder.HandlerType = typeof(InProcAuthenticationHandler);
                        });
                    });
                break;
            default:
                throw new NotImplementedException(
                    $"Authorization mode {pythagorasOpts.Value.Authorization.Mode} is not implemented yet."
                );
        }

        return serviceCollection;
    }
}
