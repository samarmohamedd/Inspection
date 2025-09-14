namespace Inspection.Domain.Enum;

/// <summary>
/// Represents different types of businesses that can be inspected
/// </summary>
public enum BusinessType
{
    /// <summary>
    /// Restaurant or food service establishment
    /// </summary>
    Restaurant = 1,
    
    /// <summary>
    /// Retail store or shop
    /// </summary>
    Retail = 2,
    
    /// <summary>
    /// Manufacturing facility
    /// </summary>
    Manufacturing = 3,
    
    /// <summary>
    /// Office building or workspace
    /// </summary>
    Office = 4,
    
    /// <summary>
    /// Healthcare facility
    /// </summary>
    Healthcare = 5,
    
    /// <summary>
    /// Educational institution
    /// </summary>
    Education = 6,
    
    /// <summary>
    /// Hospitality business (hotel, motel, etc.)
    /// </summary>
    Hospitality = 7,
    
    /// <summary>
    /// Other type of business
    /// </summary>
    Other = 8
}
