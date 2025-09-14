namespace Inspection.Domain.Enum;

/// <summary>
/// Represents the status of an inspection visit
/// </summary>
public enum InspectionStatus
{
    /// <summary>
    /// Inspection is scheduled but not yet started
    /// </summary>
    Scheduled = 1,
    
    /// <summary>
    /// Inspection is currently in progress
    /// </summary>
    InProgress = 2,
    
    /// <summary>
    /// Inspection has been completed successfully
    /// </summary>
    Completed = 3,
    
    /// <summary>
    /// Inspection was cancelled before completion
    /// </summary>
    Cancelled = 4,
    
    /// <summary>
    /// Inspection requires follow-up action
    /// </summary>
    RequiresFollowUp = 5
}
