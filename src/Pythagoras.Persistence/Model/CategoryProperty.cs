namespace Saorsa.Pythagoras.Persistence.Model;

public class CategoryProperty : PropertyBase
{
    public Guid CategoryId { get; set; }

    public Category Category { get; set; } = default!;
}
