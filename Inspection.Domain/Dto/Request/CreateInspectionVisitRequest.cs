using Inspection.Domain.Enum;

namespace Inspection.Domain.Dto.Request;

/// <summary>
/// Request DTO for creating a new inspection visit
/// </summary>
public class CreateInspectionVisitRequest
{
    /// <summary>
    /// ID of the business being inspected
    /// </summary>
    public int BusinessId { get; set; }
    
    /// <summary>
    /// ID of the inspector conducting the inspection
    /// </summary>
    public int InspectorId { get; set; }
    
    /// <summary>
    /// Date and time when the inspection is scheduled
    /// </summary>
    public DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// Type of inspection being conducted
    /// </summary>
    public InspectionType InspectionType { get; set; }
    
    /// <summary>
    /// General notes or comments about the inspection
    /// </summary>
    public string? Notes { get; set; }
}
