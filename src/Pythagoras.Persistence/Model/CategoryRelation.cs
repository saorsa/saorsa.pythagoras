namespace Saorsa.Pythagoras.Persistence.Model;

public class CategoryRelation
{
    public Guid SourceCategoryId { get; set; }

    public Category SourceCategory { get; set; } = default!;
    
    public Guid TargetCategoryId { get; set; }

    public Category TargetCategory { get; set; } = default!;

    public string RelationType { get; set; }
        = Constants.CategoryRelations.Link;
}
