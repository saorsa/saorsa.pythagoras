namespace Saorsa.Pythagoras.Persistence.Model;

public class TagMapping
{
    public Guid TagId { get; set; }

    public Tag Tag { get; set; } = default!;

    public string EntityType { get; set; } = "";
    
    public Guid EntityId { get; set; }
}
