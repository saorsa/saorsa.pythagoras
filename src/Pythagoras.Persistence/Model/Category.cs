using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saorsa.Pythagoras.Persistence.Model;

public class Category : _ModelBase<Guid>
{
    public Guid? ParentId { get; set; }
        
    public Category Parent { get; set; }

    public ICollection<Category> ChildCategories { get; set; }
        = new List<Category>();
        
    [Required, MaxLength(128)]
    public string Name { get; set; }
        
    public string? Description { get; set; }

    public uint Depth { get; set; } = 0;
        
    [Required, MaxLength(256)]
    public string AclUser { get; set; }

    [Required, MaxLength(256)]
    public string AclGroup { get; set; }

    public short AclUserPermissions { get; set; } = 0;

    public short AclGroupPermissions { get; set; } = 0;

    public short AclOtherPermissions { get; set; } = 0;

    public bool IsDisabled { get; set; } = false;

    public bool IsContainer { get; set; } = false;
        
    public string ContentNormalized { get; set; }
        
    public byte[] ContentRaw { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string ModifiedBy { get; set; }

    [InverseProperty(nameof(DataCategoryRelation.SourceCategory))]
    public ICollection<DataCategoryRelation> SourceRelations { get; set; }
        = new List<DataCategoryRelation>();

    [InverseProperty(nameof(DataCategoryRelation.TargetCategory))]
    public ICollection<DataCategoryRelation> TargetRelations { get; set; }
        = new List<DataCategoryRelation>();

    public ICollection<CategoryProperty> Properties { get; set; }
        = new List<CategoryProperty>();

    public ICollection<TagMapping> TagMappings { get; set; }
        = new List<TagMapping>();
}
