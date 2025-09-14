using Inspection.Domain.Entities;
using Inspection.Domain.Enum;

namespace Inspection.Application.Abstraction;

/// <summary>
/// Repository interface for Business entity with specific business operations
/// </summary>
public interface IBusinessRepository : IRepository<Business>
{
    /// <summary>
    /// Gets businesses by type
    /// </summary>
    /// <param name="businessType">Type of business</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of businesses</returns>
    Task<List<Business>> GetByTypeAsync(BusinessType businessType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets businesses by city
    /// </summary>
    /// <param name="city">City name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of businesses</returns>
    Task<List<Business>> GetByCityAsync(string city, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches businesses by name
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of matching businesses</returns>
    Task<List<Business>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets business with its inspection history
    /// </summary>
    /// <param name="businessId">Business ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Business with inspection visits</returns>
    Task<Business?> GetWithInspectionHistoryAsync(int businessId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets businesses that need follow-up inspections
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of businesses needing follow-up</returns>
    Task<List<Business>> GetBusinessesNeedingFollowUpAsync(CancellationToken cancellationToken = default);
}
