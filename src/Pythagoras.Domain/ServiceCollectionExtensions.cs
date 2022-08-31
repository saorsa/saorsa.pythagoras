using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Pythagoras;
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
            .AddPythagorasCoreServices()
            .AddPythagorasDomainServices();
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
            .TryAddScoped<IIdentityProvider, SimpleIdentityProvider>();
    
        serviceCollection
            .TryAddScoped<IPythagorasCategoriesService, DefaultPythagorasCategoriesService>();

        return serviceCollection;
    }
}

