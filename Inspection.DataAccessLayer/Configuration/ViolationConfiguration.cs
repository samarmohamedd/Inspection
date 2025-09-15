using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inspection.Domain.Entities;
using Inspection.Domain.Enum;

namespace Inspection.DataAccessLayer.Configuration
{
    public class ViolationConfiguration : IEntityTypeConfiguration<Violation>
    {
        public void Configure(EntityTypeBuilder<Violation> builder)
        {
            builder.ToTable("Violations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InspectionVisitId)
                .IsRequired();

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Severity)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.UpdatedBy)
                .HasMaxLength(100);

            // Relationships
            builder.HasOne(x => x.InspectionVisit)
                .WithMany(x => x.Violations)
                .HasForeignKey(x => x.InspectionVisitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
