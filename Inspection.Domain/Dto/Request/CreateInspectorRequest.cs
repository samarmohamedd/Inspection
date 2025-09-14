namespace Inspection.Domain.Dto.Request;

/// <summary>
/// Request DTO for creating a new inspector
/// </summary>
public class CreateInspectorRequest
{
    /// <summary>
    /// First name of the inspector
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Last name of the inspector
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
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
    public bool IsActive { get; set; } = true;
}
