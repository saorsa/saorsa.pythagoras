using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Saorsa.Pythagoras.Domain.Business;
using Saorsa.Pythagoras.Domain.Business.Concrete;
using Saorsa.Pythagoras.Persistence;

namespace Saorsa.Pythagoras.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPythagoras(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IIdentityProvider, SimpleIdentityProvider>()
            .AddScoped<IPythagorasCategoriesService, DefaultCategoriesService>();

        return serviceCollection;
    }
}
