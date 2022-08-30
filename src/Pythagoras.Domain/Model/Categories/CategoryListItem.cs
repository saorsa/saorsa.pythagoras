namespace Saorsa.Pythagoras.Domain.Model.Categories;

public class CategoryListItem
{
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    public string Name { get; set; } = default!;

    public string Uri { get; set; } = default!;
        
    public string? Description { get; set; }

    public uint Depth { get; set; } = 0;

    public Acl Permissions { get; set; } = default!;
}
