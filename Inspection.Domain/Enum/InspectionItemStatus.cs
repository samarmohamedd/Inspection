namespace Inspection.Domain.Enum;

/// <summary>
/// Represents the status of an individual inspection item result
/// </summary>
public enum InspectionItemStatus
{
    /// <summary>
    /// Item has not been checked yet
    /// </summary>
    NotChecked = 1,
    
    /// <summary>
    /// Item passed the inspection
    /// </summary>
    Pass = 2,
    
    /// <summary>
    /// Item failed the inspection
    /// </summary>
    Fail = 3,
    
    /// <summary>
    /// Item is not applicable for this inspection
    /// </summary>
    NotApplicable = 4,
    
    /// <summary>
    /// Item requires further review or follow-up
    /// </summary>
    RequiresReview = 5
}
