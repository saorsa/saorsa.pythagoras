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
    
    public static TService? ResolveService<TService>(
        this IServiceCollection serviceCollection, bool requireScope = true)
    {
        var provider = serviceCollection.BuildServiceProvider();
        if (requireScope)
        {
            var scope = provider.CreateScope();
            return scope.ServiceProvider.GetService<TService>();
        }

        var result = provider.GetService<TService>();
        provider.Dispose();
        return result;
    }

    public static TService ResolveRequiredService<TService>(
        this IServiceCollection serviceCollection, bool requireScope = true)
    {
        var result = ResolveService<TService>(serviceCollection, requireScope);
        if (result == null)
        {
            throw new ArgumentException(
                $"Service {typeof(TService)} is not registered in the dependency injection container.");
        }

        return result;
    }

    public static IServiceProvider CreateProvider(this IServiceCollection serviceCollection)
    {
        return serviceCollection.BuildServiceProvider();
    }
}
