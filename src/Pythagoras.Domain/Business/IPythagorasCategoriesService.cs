using Saorsa.Pythagoras.Domain.Model.Categories;

namespace Saorsa.Pythagoras.Domain.Business;

public interface IPythagorasCategoriesService : IPythagorasBusinessService
{
    Task<IEnumerable<CategoryListItem>> GetRootCategoriesAsync(
        CancellationToken cancellationToken = default);
}
