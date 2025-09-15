using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inspection.Domain.Entities;

namespace Inspection.DataAccessLayer.Configuration
{
    public class EntityToInspectConfiguration : IEntityTypeConfiguration<EntityToInspect>
    {
        public void Configure(EntityTypeBuilder<EntityToInspect> builder)
        {
            builder.ToTable("EntitiesToInspect");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.UpdatedBy)
                .HasMaxLength(100);

            // Relationships
            builder.HasMany(x => x.InspectionVisits)
                .WithOne(x => x.EntityToInspect)
                .HasForeignKey(x => x.EntityToInspectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
