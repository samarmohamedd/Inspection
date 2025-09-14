namespace Inspection.Domain.Enum;

/// <summary>
/// Represents different types of inspections that can be performed
/// </summary>
public enum InspectionType
{
    /// <summary>
    /// Routine scheduled inspection
    /// </summary>
    Routine = 1,
    
    /// <summary>
    /// Follow-up inspection after previous issues
    /// </summary>
    FollowUp = 2,
    
    /// <summary>
    /// Complaint-based inspection
    /// </summary>
    Complaint = 3,
    
    /// <summary>
    /// Emergency or urgent inspection
    /// </summary>
    Emergency = 4,
    
    /// <summary>
    /// Initial inspection for new business
    /// </summary>
    Initial = 5,
    
    /// <summary>
    /// Annual compliance inspection
    /// </summary>
    Annual = 6
}
