using Inspection.Domain.Entities;
using Inspection.Domain.Enum;

namespace Inspection.Application.Abstraction;

/// <summary>
/// Repository interface for InspectionVisit entity with specific operations
/// </summary>
public interface IInspectionVisitRepository : IRepository<InspectionVisit>
{
    /// <summary>
    /// Gets inspection visits by business ID
    /// </summary>
    /// <param name="businessId">Business ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of inspection visits</returns>
    Task<List<InspectionVisit>> GetByBusinessIdAsync(int businessId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets inspection visits by inspector ID
    /// </summary>
    /// <param name="inspectorId">Inspector ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of inspection visits</returns>
    Task<List<InspectionVisit>> GetByInspectorIdAsync(int inspectorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets inspection visits by status
    /// </summary>
    /// <param name="status">Inspection status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of inspection visits</returns>
    Task<List<InspectionVisit>> GetByStatusAsync(InspectionStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets inspection visits scheduled for a specific date range
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of inspection visits</returns>
    Task<List<InspectionVisit>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets inspection visit with all related data (business, inspector, results)
    /// </summary>
    /// <param name="visitId">Visit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Inspection visit with related data</returns>
    Task<InspectionVisit?> GetWithDetailsAsync(int visitId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets upcoming inspections for an inspector
    /// </summary>
    /// <param name="inspectorId">Inspector ID</param>
    /// <param name="days">Number of days to look ahead</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of upcoming inspection visits</returns>
    Task<List<InspectionVisit>> GetUpcomingInspectionsAsync(int inspectorId, int days = 7, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets overdue inspections that require follow-up
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of overdue inspection visits</returns>
    Task<List<InspectionVisit>> GetOverdueInspectionsAsync(CancellationToken cancellationToken = default);
}
