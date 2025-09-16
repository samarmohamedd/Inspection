using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Inspection.Domain.Entities;
using Inspection.DataAccessLayer.Configuration;

namespace Inspection.DataAccessLayer.Context
{
    public class InspectionDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public InspectionDbContext(DbContextOptions<InspectionDbContext> options) : base(options)
        {
        }

        public DbSet<Inspector> Inspectors { get; set; }
        public DbSet<EntityToInspect> EntitiesToInspect { get; set; }
        public DbSet<InspectionVisit> InspectionVisits { get; set; }
        public DbSet<Violation> Violations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfiguration());
            modelBuilder.ApplyConfiguration(new InspectorConfiguration());
            modelBuilder.ApplyConfiguration(new EntityToInspectConfiguration());
            modelBuilder.ApplyConfiguration(new InspectionVisitConfiguration());
            modelBuilder.ApplyConfiguration(new ViolationConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
