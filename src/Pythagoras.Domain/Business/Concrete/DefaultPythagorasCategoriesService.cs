using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Model.Categories;
using Saorsa.Pythagoras.Persistence;
using Serilog;

namespace Saorsa.Pythagoras.Domain.Business.Concrete;

public class DefaultPythagorasCategoriesService : IPythagorasCategoriesService
{
    public IIdentityProvider IdentityProvider { get; }

    public PythagorasDbContext DbContext { get; }
    public ILogger<DefaultPythagorasCategoriesService> Logger { get; }
    public IPythagorasMapperProvider PythagorasMapperProvider { get; }
    public IMapper Mapper => PythagorasMapperProvider.Mapper;

    public DefaultPythagorasCategoriesService(
        IIdentityProvider idProvider,
        IPythagorasMapperProvider pythagorasMapperProvider,
        PythagorasDbContext dbContext,
        ILogger<DefaultPythagorasCategoriesService> logger)
    {
        IdentityProvider = idProvider;
        DbContext = dbContext;
        Logger = logger;
        PythagorasMapperProvider = pythagorasMapperProvider;
    }

    public async Task<IEnumerable<CategoryListItem>> GetRootCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        CheckSessionUserAndWarn();
        var dbResults = await DbContext.Categories
            .Where(c => !c.ParentId.HasValue)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
        return Mapper.Map<IEnumerable<CategoryListItem>>(dbResults);
    }

    void CheckSessionUserAndWarn()
    {
        if (!IdentityProvider.IsLoggedIn)
        {
            Logger.LogWarning("Anonymous access");
        }
        else
        {
            var id = IdentityProvider.GetLoggedInUser();
            Logger.LogDebug("Access from {User}, Groups = {Groups}", 
                id!.User,
                id.Groups);
        }
    }
}
