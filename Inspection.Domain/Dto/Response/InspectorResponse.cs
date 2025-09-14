namespace Inspection.Domain.Dto.Response;

/// <summary>
/// Response DTO for inspector data
/// </summary>
public class InspectorResponse
{
    /// <summary>
    /// Unique identifier for the inspector
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// First name of the inspector
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Last name of the inspector
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Full name of the inspector
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Inspector's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Inspector's phone number
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Inspector's employee ID or badge number
    /// </summary>
    public string? EmployeeId { get; set; }
    
    /// <summary>
    /// Inspector's department or division
    /// </summary>
    public string? Department { get; set; }
    
    /// <summary>
    /// Inspector's certification number or license
    /// </summary>
    public string? CertificationNumber { get; set; }
    
    /// <summary>
    /// Date when the inspector's certification expires
    /// </summary>
    public DateTime? CertificationExpiryDate { get; set; }
    
    /// <summary>
    /// Indicates whether the inspector is currently active
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Indicates whether the certification is expired
    /// </summary>
    public bool IsCertificationExpired { get; set; }
    
    /// <summary>
    /// Date when the inspector was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date when the inspector was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Total number of inspections conducted by this inspector
    /// </summary>
    public int TotalInspections { get; set; }
    
    /// <summary>
    /// Date of the last inspection conducted
    /// </summary>
    public DateTime? LastInspectionDate { get; set; }
}
