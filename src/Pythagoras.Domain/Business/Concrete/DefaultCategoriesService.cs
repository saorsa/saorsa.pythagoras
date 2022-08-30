using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Saorsa.Pythagoras.Domain.Model.Categories;
using Saorsa.Pythagoras.Persistence;

namespace Saorsa.Pythagoras.Domain.Business.Concrete;

public class DefaultCategoriesService : IPythagorasCategoriesService
{
    public IIdentityProvider IdentityProvider { get; }

    public PythagorasDbContext DbContext { get; }
    public IMapper Mapper { get; }

    public DefaultCategoriesService(
        IIdentityProvider idProvider,
        PythagorasDbContext dbContext,
        IMapper mapper)
    {
        IdentityProvider = idProvider;
        DbContext = dbContext;
        Mapper = mapper;
    }

    public async Task<IEnumerable<CategoryListItem>> GetRootCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var dbResults = await DbContext.Categories
            .Where(c => !c.ParentId.HasValue)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<CategoryListItem>>(dbResults);
    }
}
