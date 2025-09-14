using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inspection.Domain.Entities;
using Inspection.Domain.Enum;

namespace Inspection.DataAccessLayer.Configuration;

/// <summary>
/// Entity configuration for Business entity
/// </summary>
public class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
    /// <summary>
    /// Configures the Business entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        // Table name
        builder.ToTable("Businesses");

        // Primary key
        builder.HasKey(b => b.Id);

        // Properties
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.RegistrationNumber)
            .HasMaxLength(50);

        builder.Property(b => b.BusinessType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(b => b.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(b => b.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.State)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.Phone)
            .HasMaxLength(20);

        builder.Property(b => b.Email)
            .HasMaxLength(200);

        builder.Property(b => b.OwnerName)
            .HasMaxLength(200);

        builder.Property(b => b.Notes)
            .HasMaxLength(1000);

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        builder.Property(b => b.UpdatedAt)
            .IsRequired();

        builder.Property(b => b.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Relationships
        builder.HasMany(b => b.InspectionVisits)
            .WithOne(iv => iv.Business)
            .HasForeignKey(iv => iv.BusinessId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(b => b.Name);
        builder.HasIndex(b => b.BusinessType);
        builder.HasIndex(b => b.City);
        builder.HasIndex(b => b.RegistrationNumber);
    }
}
