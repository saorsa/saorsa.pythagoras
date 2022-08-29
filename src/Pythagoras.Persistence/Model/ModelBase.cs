namespace Saorsa.Pythagoras.Persistence.Model;

public abstract class ModelBase<TId> where TId : IComparable<TId>, IComparable
{
    public TId Id { get; set; } = default!;
        
    public DateTimeOffset CreatedAt { get; set; }
        = DateTimeOffset.UtcNow;

    public string CreatedBy { get; set; }
        = Constants.Identities.System;

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }
}
