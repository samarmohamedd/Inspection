using Inspection.Domain.Enum;

namespace Inspection.Domain.Dto.Response;

/// <summary>
/// Summary response DTO for business list operations
/// </summary>
public class BusinessSummaryResponse
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
    /// Type of business
    /// </summary>
    public BusinessType BusinessType { get; set; }
    
    /// <summary>
    /// Business type as string for display
    /// </summary>
    public string BusinessTypeDisplay { get; set; } = string.Empty;
    
    /// <summary>
    /// City where the business is located
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// State or province where the business is located
    /// </summary>
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// Business phone number
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Total number of inspections conducted for this business
    /// </summary>
    public int TotalInspections { get; set; }
    
    /// <summary>
    /// Date of the last inspection
    /// </summary>
    public DateTime? LastInspectionDate { get; set; }
    
    /// <summary>
    /// Status of the last inspection
    /// </summary>
    public InspectionStatus? LastInspectionStatus { get; set; }
    
    /// <summary>
    /// Average score from all completed inspections
    /// </summary>
    public decimal? AverageScore { get; set; }
}
