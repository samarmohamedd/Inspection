using Inspection.Domain.Enum;

namespace Inspection.Domain.Dto.Response;

/// <summary>
/// Response DTO for business data
/// </summary>
public class BusinessResponse
{
    /// <summary>
    /// Unique identifier for the business
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Name of the business
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Business registration or license number
    /// </summary>
    public string? RegistrationNumber { get; set; }
    
    /// <summary>
    /// Type of business
    /// </summary>
    public BusinessType BusinessType { get; set; }
    
    /// <summary>
    /// Business type as string for display
    /// </summary>
    public string BusinessTypeDisplay { get; set; } = string.Empty;
    
    /// <summary>
    /// Complete address of the business
    /// </summary>
    public string FullAddress { get; set; } = string.Empty;
    
    /// <summary>
    /// Street address of the business
    /// </summary>
    public string Address { get; set; } = string.Empty;
    
    /// <summary>
    /// City where the business is located
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// State or province where the business is located
    /// </summary>
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// Postal or ZIP code
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Business phone number
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Business email address
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Name of the business owner or manager
    /// </summary>
    public string? OwnerName { get; set; }
    
    /// <summary>
    /// Additional notes about the business
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Date when the business was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date when the business was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Total number of inspections conducted for this business
    /// </summary>
    public int TotalInspections { get; set; }
    
    /// <summary>
    /// Date of the last inspection
    /// </summary>
    public DateTime? LastInspectionDate { get; set; }
}
