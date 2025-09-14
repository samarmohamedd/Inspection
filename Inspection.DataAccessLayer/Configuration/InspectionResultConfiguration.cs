using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inspection.Domain.Entities;

namespace Inspection.DataAccessLayer.Configuration;

/// <summary>
/// Entity configuration for InspectionResult entity
/// </summary>
public class InspectionResultConfiguration : IEntityTypeConfiguration<InspectionResult>
{
    /// <summary>
    /// Configures the InspectionResult entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<InspectionResult> builder)
    {
        // Table name
        builder.ToTable("InspectionResults");

        // Primary key
        builder.HasKey(ir => ir.Id);

        // Properties
        builder.Property(ir => ir.InspectionVisitId)
            .IsRequired();

        builder.Property(ir => ir.InspectionItemId)
            .IsRequired();

        builder.Property(ir => ir.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(ir => ir.Score)
            .HasPrecision(5, 2);

        builder.Property(ir => ir.Notes)
            .HasMaxLength(1000);

        builder.Property(ir => ir.CorrectiveAction)
            .HasMaxLength(1000);

        builder.Property(ir => ir.CorrectiveActionDueDate);

        builder.Property(ir => ir.HasPhotographicEvidence)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ir => ir.PhotoPath)
            .HasMaxLength(500);

        builder.Property(ir => ir.CreatedAt)
            .IsRequired();

        builder.Property(ir => ir.UpdatedAt)
            .IsRequired();

        builder.Property(ir => ir.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(ir => ir.InspectionVisit)
            .WithMany(iv => iv.InspectionResults)
            .HasForeignKey(ir => ir.InspectionVisitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ir => ir.InspectionItem)
            .WithMany(ii => ii.InspectionResults)
            .HasForeignKey(ir => ir.InspectionItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(ir => ir.InspectionVisitId);
        builder.HasIndex(ir => ir.InspectionItemId);
        builder.HasIndex(ir => ir.Status);

        // Composite unique index to prevent duplicate results for the same visit/item combination
        builder.HasIndex(ir => new { ir.InspectionVisitId, ir.InspectionItemId })
            .IsUnique();
    }
}
