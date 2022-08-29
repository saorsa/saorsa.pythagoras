using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saorsa.Pythagoras.Persistence.Model;

public class Category : ModelBase<Guid>
{
    public Guid? ParentId { get; set; }

    public Category Parent { get; set; } = default!;

    [Required, MaxLength(128)]
    public string Name { get; set; }
        = $"Category-{Guid.NewGuid():N}";

    [Required]
    public string Uri { get; set; }
        = string.Empty;
        
    public string? Description { get; set; }

    public uint Depth { get; set; } = 0;

    [Required, MaxLength(256)]
    public string User { get; set; }
        = Constants.Identities.None;

    [Required, MaxLength(256)]
    public string Group { get; set; }
        = Constants.Identities.None;

    public short UserMask { get; set; } = 0;

    public short GroupMask { get; set; } = 0;

    public short OtherMask { get; set; } = 0;

    public ICollection<Category> Children { get; set; }
        = new List<Category>();

    public ICollection<CategoryProperty> Properties { get; set; }
        = new List<CategoryProperty>();

    [InverseProperty(nameof(CategoryRelation.SourceCategory))]
    public ICollection<CategoryRelation> SourceRelations { get; set; }
        = new List<CategoryRelation>();

    [InverseProperty(nameof(CategoryRelation.TargetCategory))]
    public ICollection<CategoryRelation> TargetRelations { get; set; }
        = new List<CategoryRelation>();
}
