using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Auth;
using Saorsa.Pythagoras.Domain.Model.Categories;
using Saorsa.Pythagoras.Persistence;

namespace Saorsa.Pythagoras.Domain.Business.Concrete;

public class DefaultPythagorasCategoriesService : IPythagorasCategoriesService
{
    public IPythagorasIdentityProvider IdentityProvider { get; }

    public PythagorasDbContext DbContext { get; }
    public ILogger<DefaultPythagorasCategoriesService> Logger { get; }
    public IPythagorasMapperProvider PythagorasMapperProvider { get; }
    public IMapper Mapper => PythagorasMapperProvider.Mapper;

    public DefaultPythagorasCategoriesService(
        IPythagorasIdentityProvider idProvider,
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
        CheckSessionUserOrDie();
        var dbResults = await DbContext.Categories
            .Where(c => !c.ParentId.HasValue)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
        return Mapper.Map<IEnumerable<CategoryListItem>>(dbResults);
    }

    void CheckSessionUserOrDie()
    {
        if (!IdentityProvider.IsLoggedIn)
        {
            Logger.LogCritical("Anonymous access is not allowed");
            throw new PythagorasException(
                ErrorCodes.Auth.Unauthorized,
                "No user session found.");
        }
        else
        {
            var id = IdentityProvider.GetLoggedInUser();
            Logger.LogInformation("Access from {User}, Groups = {Groups}", 
                id!.User,
                id.Groups);
        }
    }
}
