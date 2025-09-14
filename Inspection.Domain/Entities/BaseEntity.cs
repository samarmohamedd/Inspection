namespace Inspection.Domain.Entities;

/// <summary>
/// Base entity class that provides common properties for all entities
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date and time when the entity was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Indicates whether the entity is soft deleted
    /// </summary>
    public bool IsDeleted { get; set; }
}
