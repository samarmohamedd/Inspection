using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inspection.Domain.Entities;
using Inspection.Domain.Enum;

namespace Inspection.DataAccessLayer.Configuration
{
    public class InspectionVisitConfiguration : IEntityTypeConfiguration<InspectionVisit>
    {
        public void Configure(EntityTypeBuilder<InspectionVisit> builder)
        {
            builder.ToTable("InspectionVisits");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EntityToInspectId)
                .IsRequired();

            builder.Property(x => x.InspectorId)
                .IsRequired();

            builder.Property(x => x.ScheduledAt)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(InspectionStatus.Planned); 

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            builder.Property(x => x.CompletedAt);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.UpdatedBy)
                .HasMaxLength(100);

            // Relationships
            builder.HasOne(x => x.EntityToInspect)
                .WithMany(x => x.InspectionVisits)
                .HasForeignKey(x => x.EntityToInspectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Inspector)
                .WithMany(x => x.InspectionVisits)
                .HasForeignKey(x => x.InspectorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Violations)
                .WithOne(x => x.InspectionVisit)
                .HasForeignKey(x => x.InspectionVisitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
