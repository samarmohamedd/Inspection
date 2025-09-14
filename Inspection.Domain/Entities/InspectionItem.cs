namespace Inspection.Domain.Entities;

/// <summary>
/// Represents an individual item or criterion that can be checked during an inspection
/// </summary>
public class InspectionItem : BaseEntity
{
    /// <summary>
    /// Name or title of the inspection item
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed description of what needs to be inspected
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Category or group this inspection item belongs to
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// Indicates whether this item is required for all inspections
    /// </summary>
    public bool IsRequired { get; set; } = true;
    
    /// <summary>
    /// Display order for this item in the inspection checklist
    /// </summary>
    public int DisplayOrder { get; set; }
    
    /// <summary>
    /// Indicates whether this inspection item is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Additional notes or instructions for the inspector
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Navigation property for inspection results related to this item
    /// </summary>
    public virtual ICollection<InspectionResult> InspectionResults { get; set; } = new List<InspectionResult>();
}
