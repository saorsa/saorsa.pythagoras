using System.ComponentModel.DataAnnotations;

namespace Saorsa.Pythagoras.Persistence.Model;

public class PropertyBase : ModelBase<Guid>
{
    [Required, MaxLength(256)] public string Name { get; set; }
        = $"Property-{Guid.NewGuid():N}";

    public bool IsRequired { get; set; }
        = false;
        
    [Required, MaxLength(256)]
    public string? ValueType { get; set; }

    public string? Value { get; set; }
}
