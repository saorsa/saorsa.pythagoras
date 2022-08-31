using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Pythagoras.Logging;

namespace Pythagoras;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPythagorasCoreServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddLogging();
           //.TryAddSingleton<ILoggerFactory>(_ => new PythagorasLoggingFactory());

        return serviceCollection;
    }
}
