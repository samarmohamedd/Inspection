using Microsoft.EntityFrameworkCore;
using Inspection.Domain.Entities;

namespace Inspection.DataAccessLayer.Context;

/// <summary>
/// Application database context for the Inspection system
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the ApplicationDbContext
    /// </summary>
    /// <param name="options">Database context options</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet for Business entities
    /// </summary>
    public DbSet<Business> Businesses { get; set; }

    /// <summary>
    /// DbSet for Inspector entities
    /// </summary>
    public DbSet<Inspector> Inspectors { get; set; }

    /// <summary>
    /// DbSet for InspectionVisit entities
    /// </summary>
    public DbSet<InspectionVisit> InspectionVisits { get; set; }

    /// <summary>
    /// DbSet for InspectionItem entities
    /// </summary>
    public DbSet<InspectionItem> InspectionItems { get; set; }

    /// <summary>
    /// DbSet for InspectionResult entities
    /// </summary>
    public DbSet<InspectionResult> InspectionResults { get; set; }

    /// <summary>
    /// Configures the model and relationships
    /// </summary>
    /// <param name="modelBuilder">Model builder instance</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configure soft delete filter for all entities inheriting from BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(e => !((BaseEntity)e).IsDeleted);
            }
        }
    }

    /// <summary>
    /// Override SaveChanges to automatically set audit fields
    /// </summary>
    /// <returns>Number of affected records</returns>
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync to automatically set audit fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected records</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates audit fields for entities being added or modified
    /// </summary>
    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
