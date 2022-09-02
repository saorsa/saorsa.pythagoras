using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Saorsa.Pythagoras.Domain.Auth;
using Saorsa.Pythagoras.Domain.Business;
using Saorsa.Pythagoras.Domain.Business.Concrete;
using Saorsa.Pythagoras.Persistence;
using Saorsa.Pythagoras.Persistence.Npgsql;

namespace Saorsa.Pythagoras.Domain;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPythagoras(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddPythagoras(configAction: null);
    }

    public static IServiceCollection AddPythagoras(
        this IServiceCollection serviceCollection,
        Action<PythagorasOptions>? configAction)
    {
        return serviceCollection
            .ConfigurePythagoras(configAction)
            .AddPythagorasCoreServices()
            .AddPythagorasAuthServices()
            .AddPythagorasDomainServices();
    }
    
    public static IServiceCollection AddPythagoras(
        this IServiceCollection serviceCollection,
        string configSectionPath)
    {
        return serviceCollection
            .ConfigurePythagoras(configSectionPath)
            .AddPythagorasCoreServices()
            .AddPythagorasAuthServices()
            .AddPythagorasDomainServices();
    }

    public static IServiceCollection ConfigurePythagoras(
        this IServiceCollection serviceCollection,
        Action<PythagorasOptions>? configAction)
    {
        serviceCollection.AddOptions<PythagorasOptions>()
            .Configure(configAction ?? (options =>
            {
                options.InvalidateFrom(new PythagorasOptions());
            }));
        return serviceCollection;
    }
    
    public static IServiceCollection ConfigurePythagoras(
        this IServiceCollection serviceCollection,
        string configSectionPath)
    {
        serviceCollection
            .AddOptions<PythagorasOptions>()
            .BindConfiguration(configSectionPath);
        return serviceCollection;
    }

    public static IServiceCollection AddPythagorasDomainServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .TryAddSingleton<IPythagorasMapperProvider, DefaultPythagorasMapperProvider>();

        serviceCollection
            .AddDbContext<PythagorasDbContext>(opts =>
            {
                opts.UsePythagorasWithNpgSql();
            });
    
        serviceCollection
            .TryAddScoped<IPythagorasCategoriesService, DefaultPythagorasCategoriesService>();

        return serviceCollection;
    }
}
