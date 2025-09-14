using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inspection.Domain.Entities;

namespace Inspection.DataAccessLayer.Configuration;

/// <summary>
/// Entity configuration for InspectionItem entity
/// </summary>
public class InspectionItemConfiguration : IEntityTypeConfiguration<InspectionItem>
{
    /// <summary>
    /// Configures the InspectionItem entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<InspectionItem> builder)
    {
        // Table name
        builder.ToTable("InspectionItems");

        // Primary key
        builder.HasKey(ii => ii.Id);

        // Properties
        builder.Property(ii => ii.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ii => ii.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(ii => ii.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ii => ii.IsRequired)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(ii => ii.DisplayOrder)
            .IsRequired();

        builder.Property(ii => ii.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(ii => ii.Notes)
            .HasMaxLength(500);

        builder.Property(ii => ii.CreatedAt)
            .IsRequired();

        builder.Property(ii => ii.UpdatedAt)
            .IsRequired();

        builder.Property(ii => ii.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Relationships
        builder.HasMany(ii => ii.InspectionResults)
            .WithOne(ir => ir.InspectionItem)
            .HasForeignKey(ir => ir.InspectionItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(ii => ii.Category);
        builder.HasIndex(ii => ii.IsActive);
        builder.HasIndex(ii => ii.DisplayOrder);
    }
}
