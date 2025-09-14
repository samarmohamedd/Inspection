using Inspection.Domain.Enum;

namespace Inspection.Domain.Dto.Request;

/// <summary>
/// Request DTO for updating an existing business
/// </summary>
public class UpdateBusinessRequest
{
    /// <summary>
    /// ID of the business to update
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Name of the business
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Business registration or license number
    /// </summary>
    public string? RegistrationNumber { get; set; }
    
    /// <summary>
    /// Type of business
    /// </summary>
    public BusinessType BusinessType { get; set; }
    
    /// <summary>
    /// Street address of the business
    /// </summary>
    public string Address { get; set; } = string.Empty;
    
    /// <summary>
    /// City where the business is located
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// State or province where the business is located
    /// </summary>
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// Postal or ZIP code
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Business phone number
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Business email address
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Name of the business owner or manager
    /// </summary>
    public string? OwnerName { get; set; }
    
    /// <summary>
    /// Additional notes about the business
    /// </summary>
    public string? Notes { get; set; }
}
