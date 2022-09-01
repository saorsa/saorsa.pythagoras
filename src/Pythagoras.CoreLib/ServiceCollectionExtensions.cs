using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Pythagoras.Logging;

namespace Pythagoras;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPythagorasCoreServices(this IServiceCollection serviceCollection)
    {
        if (!serviceCollection.IsServiceRegistered<ILoggerFactory>())
        {
            serviceCollection
                .AddLogging()
                .AddSingleton<ILoggerFactory>(_ => new PythagorasLoggingFactory());
        }

        return serviceCollection;
    }

    public static bool IsServiceRegistered<T>(this IServiceCollection serviceCollection)
    {
        return serviceCollection.Any(s => s.ServiceType == typeof(T));
    }
}
