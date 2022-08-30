using Microsoft.EntityFrameworkCore;
using Saorsa.Pythagoras.Persistence.Model;

namespace Saorsa.Pythagoras.Persistence;

public class PythagorasDbContext : DbContext
{
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TagMapping> TagMappings => Set<TagMapping>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CategoryProperty> CategoryProperties => Set<CategoryProperty>();
    public DbSet<CategoryRelation> CategoryRelations => Set<CategoryRelation>();
    
    public PythagorasDbContext(DbContextOptions<PythagorasDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CategoryRelation>(e =>
        {
            e.HasKey(relation => new
            {
                relation.SourceCategoryId,
                relation.TargetCategoryId,
                relation.RelationType
            });
        });
        
        modelBuilder.Entity<TagMapping>(e =>
        {
            e.HasKey(mapping => new
            {
                mapping.TagId,
                mapping.EntityId,
                mapping.EntityType
            });
        });
    }
}
