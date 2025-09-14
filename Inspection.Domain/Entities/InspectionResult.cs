using Inspection.Domain.Enum;

namespace Inspection.Domain.Entities;

/// <summary>
/// Represents the result of checking a specific inspection item during an inspection visit
/// </summary>
public class InspectionResult : BaseEntity
{
    /// <summary>
    /// Foreign key to the inspection visit this result belongs to
    /// </summary>
    public int InspectionVisitId { get; set; }
    
    /// <summary>
    /// Foreign key to the inspection item being checked
    /// </summary>
    public int InspectionItemId { get; set; }
    
    /// <summary>
    /// Status of this inspection item (Pass, Fail, etc.)
    /// </summary>
    public InspectionItemStatus Status { get; set; }
    
    /// <summary>
    /// Score or rating for this specific item (0-100)
    /// </summary>
    public decimal? Score { get; set; }
    
    /// <summary>
    /// Detailed notes or comments about this inspection item
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Corrective action required if the item failed
    /// </summary>
    public string? CorrectiveAction { get; set; }
    
    /// <summary>
    /// Date by which the corrective action must be completed
    /// </summary>
    public DateTime? CorrectiveActionDueDate { get; set; }
    
    /// <summary>
    /// Indicates whether photographic evidence was taken
    /// </summary>
    public bool HasPhotographicEvidence { get; set; }
    
    /// <summary>
    /// Path or URL to photographic evidence (if any)
    /// </summary>
    public string? PhotoPath { get; set; }
    
    /// <summary>
    /// Navigation property to the inspection visit
    /// </summary>
    public virtual InspectionVisit InspectionVisit { get; set; } = null!;
    
    /// <summary>
    /// Navigation property to the inspection item
    /// </summary>
    public virtual InspectionItem InspectionItem { get; set; } = null!;
}
