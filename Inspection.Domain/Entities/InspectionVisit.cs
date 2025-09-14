using Inspection.Domain.Enum;

namespace Inspection.Domain.Entities;

/// <summary>
/// Represents an inspection visit to a business
/// </summary>
public class InspectionVisit : BaseEntity
{
    /// <summary>
    /// Foreign key to the business being inspected
    /// </summary>
    public int BusinessId { get; set; }
    
    /// <summary>
    /// Foreign key to the inspector conducting the inspection
    /// </summary>
    public int InspectorId { get; set; }
    
    /// <summary>
    /// Date and time when the inspection is scheduled or was conducted
    /// </summary>
    public DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// Type of inspection being conducted
    /// </summary>
    public InspectionType InspectionType { get; set; }
    
    /// <summary>
    /// Current status of the inspection
    /// </summary>
    public InspectionStatus Status { get; set; }
    
    /// <summary>
    /// Date and time when the inspection started
    /// </summary>
    public DateTime? StartTime { get; set; }
    
    /// <summary>
    /// Date and time when the inspection ended
    /// </summary>
    public DateTime? EndTime { get; set; }
    
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
    /// Navigation property to the business being inspected
    /// </summary>
    public virtual Business Business { get; set; } = null!;
    
    /// <summary>
    /// Navigation property to the inspector conducting the inspection
    /// </summary>
    public virtual Inspector Inspector { get; set; } = null!;
    
    /// <summary>
    /// Navigation property for inspection results
    /// </summary>
    public virtual ICollection<InspectionResult> InspectionResults { get; set; } = new List<InspectionResult>();
}
