using System.ComponentModel.DataAnnotations;

namespace Saorsa.Pythagoras.Persistence.Model;

public class Tag : ModelBase<Guid>
{
    [Required, MaxLength(256)]
    public string Name { get; set; }
        = $"Tag-{Guid.NewGuid():N}";

    public ICollection<TagMapping> Mappings { get; }
        = new List<TagMapping>();
}
