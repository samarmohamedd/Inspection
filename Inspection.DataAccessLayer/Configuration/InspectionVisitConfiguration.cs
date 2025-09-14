using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inspection.Domain.Entities;

namespace Inspection.DataAccessLayer.Configuration;

/// <summary>
/// Entity configuration for InspectionVisit entity
/// </summary>
public class InspectionVisitConfiguration : IEntityTypeConfiguration<InspectionVisit>
{
    /// <summary>
    /// Configures the InspectionVisit entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<InspectionVisit> builder)
    {
        // Table name
        builder.ToTable("InspectionVisits");

        // Primary key
        builder.HasKey(iv => iv.Id);

        // Properties
        builder.Property(iv => iv.BusinessId)
            .IsRequired();

        builder.Property(iv => iv.InspectorId)
            .IsRequired();

        builder.Property(iv => iv.InspectionDate)
            .IsRequired();

        builder.Property(iv => iv.InspectionType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(iv => iv.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(iv => iv.StartTime);

        builder.Property(iv => iv.EndTime);

        builder.Property(iv => iv.OverallScore)
            .HasPrecision(5, 2);

        builder.Property(iv => iv.Notes)
            .HasMaxLength(2000);

        builder.Property(iv => iv.Summary)
            .HasMaxLength(2000);

        builder.Property(iv => iv.FollowUpDate);

        builder.Property(iv => iv.CreatedAt)
            .IsRequired();

        builder.Property(iv => iv.UpdatedAt)
            .IsRequired();

        builder.Property(iv => iv.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(iv => iv.Business)
            .WithMany(b => b.InspectionVisits)
            .HasForeignKey(iv => iv.BusinessId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(iv => iv.Inspector)
            .WithMany(i => i.InspectionVisits)
            .HasForeignKey(iv => iv.InspectorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(iv => iv.InspectionResults)
            .WithOne(ir => ir.InspectionVisit)
            .HasForeignKey(ir => ir.InspectionVisitId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(iv => iv.BusinessId);
        builder.HasIndex(iv => iv.InspectorId);
        builder.HasIndex(iv => iv.InspectionDate);
        builder.HasIndex(iv => iv.Status);
        builder.HasIndex(iv => iv.InspectionType);
    }
}
