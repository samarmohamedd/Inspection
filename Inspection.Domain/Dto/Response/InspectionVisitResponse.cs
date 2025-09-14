using Inspection.Domain.Enum;

namespace Inspection.Domain.Dto.Response;

/// <summary>
/// Response DTO for inspection visit data
/// </summary>
public class InspectionVisitResponse
{
    /// <summary>
    /// Unique identifier for the inspection visit
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// ID of the business being inspected
    /// </summary>
    public int BusinessId { get; set; }
    
    /// <summary>
    /// Name of the business being inspected
    /// </summary>
    public string BusinessName { get; set; } = string.Empty;
    
    /// <summary>
    /// ID of the inspector conducting the inspection
    /// </summary>
    public int InspectorId { get; set; }
    
    /// <summary>
    /// Name of the inspector conducting the inspection
    /// </summary>
    public string InspectorName { get; set; } = string.Empty;
    
    /// <summary>
    /// Date and time when the inspection is scheduled or was conducted
    /// </summary>
    public DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// Type of inspection being conducted
    /// </summary>
    public InspectionType InspectionType { get; set; }
    
    /// <summary>
    /// Inspection type as string for display
    /// </summary>
    public string InspectionTypeDisplay { get; set; } = string.Empty;
    
    /// <summary>
    /// Current status of the inspection
    /// </summary>
    public InspectionStatus Status { get; set; }
    
    /// <summary>
    /// Status as string for display
    /// </summary>
    public string StatusDisplay { get; set; } = string.Empty;
    
    /// <summary>
    /// Date and time when the inspection started
    /// </summary>
    public DateTime? StartTime { get; set; }
    
    /// <summary>
    /// Date and time when the inspection ended
    /// </summary>
    public DateTime? EndTime { get; set; }
    
    /// <summary>
    /// Duration of the inspection in minutes
    /// </summary>
    public int? DurationMinutes { get; set; }
    
    /// <summary>
    /// Overall score or rating for the inspection (0-100)
    /// </summary>
    public decimal? OverallScore { get; set; }
    
    /// <summary>
    /// General notes or comments about the inspection
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Summary of findings or recommendations
    /// </summary>
    public string? Summary { get; set; }
    
    /// <summary>
    /// Date by which any issues must be resolved
    /// </summary>
    public DateTime? FollowUpDate { get; set; }
    
    /// <summary>
    /// Date when the inspection visit was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date when the inspection visit was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Number of inspection items checked
    /// </summary>
    public int TotalItems { get; set; }
    
    /// <summary>
    /// Number of items that passed
    /// </summary>
    public int PassedItems { get; set; }
    
    /// <summary>
    /// Number of items that failed
    /// </summary>
    public int FailedItems { get; set; }
}
