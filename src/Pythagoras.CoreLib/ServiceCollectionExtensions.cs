using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.InterProcess;
using Saorsa.Pythagoras.Logging;

namespace Saorsa.Pythagoras;

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

        return serviceCollection
            .AddScoped<IProcessRunner, ProcessRunner>();
    }

    public static bool IsServiceRegistered<T>(this IServiceCollection serviceCollection)
    {
        return serviceCollection.Any(s => s.ServiceType == typeof(T));
    }
}
