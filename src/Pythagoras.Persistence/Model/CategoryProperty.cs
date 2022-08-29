namespace Saorsa.Pythagoras.Persistence.Model;

public class CategoryProperty : _PropertyBase
{
    public Guid CategoryId { get; set; }

    public Category Category { get; set; } = default!;
}
