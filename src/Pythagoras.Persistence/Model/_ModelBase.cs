namespace Saorsa.Pythagoras.Persistence.Model;

public abstract class _ModelBase<TId> where TId : IComparable<TId>, IComparable
{
    public TId Id { get; set; } = default!;
        
    public DateTimeOffset CreatedAt { get; set; }
        = DateTimeOffset.UtcNow;

    public string CreatedBy { get; set; }
        = Constants.DefaultCreatedBy;

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }
}
