using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Saorsa.Pythagoras.Domain.Auth;
using Saorsa.Pythagoras.Domain.Business;
using Saorsa.Pythagoras.Domain.Business.Concrete;
using Saorsa.Pythagoras.Domain.Configuration;
using Saorsa.Pythagoras.Persistence;
using Saorsa.Pythagoras.Persistence.Npgsql;

namespace Saorsa.Pythagoras.Domain;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPythagoras(
        this IServiceCollection serviceCollection,
        PythagorasConfiguration config)
    {
        return serviceCollection
            .ConfigurePythagoras(config)
            .AddPythagorasCoreServices()
            .AddPythagorasAuthServices()
            .AddPythagorasDomainServices();
    }
    
    public static IServiceCollection AddPythagoras(
        this IServiceCollection serviceCollection,
        string configSectionPath = "Pythagoras")
    {
        var configRoot = PythagorasRuntime.GetConfigurationFromAppSettings();
        var configSection = configRoot.GetSection(configSectionPath);
        var pythagorasConfig = new PythagorasConfiguration();
        configSection.Bind(pythagorasConfig);
        
        return serviceCollection
            .ConfigurePythagoras(pythagorasConfig)
            .AddPythagorasCoreServices()
            .AddPythagorasAuthServices()
            .AddPythagorasDomainServices();
    }

    public static IServiceCollection ConfigurePythagoras(
        this IServiceCollection serviceCollection,
        PythagorasConfiguration config)
    {
        serviceCollection
            .AddSingleton(config)
            .AddOptions<PythagorasConfiguration>()
            .Configure(options => { options.InvalidateFrom(config); });
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
