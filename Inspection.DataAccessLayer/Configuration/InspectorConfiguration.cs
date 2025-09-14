using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inspection.Domain.Entities;

namespace Inspection.DataAccessLayer.Configuration;

/// <summary>
/// Entity configuration for Inspector entity
/// </summary>
public class InspectorConfiguration : IEntityTypeConfiguration<Inspector>
{
    /// <summary>
    /// Configures the Inspector entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Inspector> builder)
    {
        // Table name
        builder.ToTable("Inspectors");

        // Primary key
        builder.HasKey(i => i.Id);

        // Properties
        builder.Property(i => i.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Phone)
            .HasMaxLength(20);

        builder.Property(i => i.EmployeeId)
            .HasMaxLength(50);

        builder.Property(i => i.Department)
            .HasMaxLength(100);

        builder.Property(i => i.CertificationNumber)
            .HasMaxLength(100);

        builder.Property(i => i.CertificationExpiryDate);

        builder.Property(i => i.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.Property(i => i.UpdatedAt)
            .IsRequired();

        builder.Property(i => i.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Computed column for FullName (not stored in database)
        builder.Ignore(i => i.FullName);

        // Relationships
        builder.HasMany(i => i.InspectionVisits)
            .WithOne(iv => iv.Inspector)
            .HasForeignKey(iv => iv.InspectorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(i => i.Email)
            .IsUnique();
        builder.HasIndex(i => i.EmployeeId);
        builder.HasIndex(i => i.IsActive);
    }
}
